using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Movies.Api;
using Movies.Application.AuthModels;
using Movies.Application.Data;
using Movies.Application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

public class AuthenticationController:ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly MoviesContext _dbContext;
    
    
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthenticationController(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        MoviesContext dbContext,
        TokenValidationParameters tokenValidationParameters)
    {
        _userManager= userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _dbContext = dbContext;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpPost(ApiEndpoints.Users.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterVM payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please provide all required fields");
        }
        var userExist = await _userManager.FindByEmailAsync(payload.Email);
        if (userExist != null)
        {
            return BadRequest($"User {payload.Email} already exists");
        }

        User user = new User()
        {
            Email = payload.Email,
            UserName = payload.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
            Name = payload.Name,
            LastName = payload.LastName



        };
        var result=await _userManager.CreateAsync(user, payload.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { errors });

            
        }

        switch (payload.Role)
        {
            case "Admin":
                await _userManager.AddToRoleAsync(user, "Admin");
                break;
            case "User":
                await _userManager.AddToRoleAsync(user, "User");
                break;
        }
        
        return Created(nameof(Register), $"User {payload.Email} created");


    }

    [HttpPost(ApiEndpoints.Users.Login)]
    public async Task<IActionResult> Login([FromBody] LoginVM payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please provide all required fields");
        }
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, payload.Password))
        {
            var tokenValue= await GenerateJwtTokenAsync(user,"");
            return Ok(tokenValue);
            
        }
        return Unauthorized();
    }

    [HttpPost(ApiEndpoints.Users.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVm payload)
    {
        try
        {
            var result=await VerifyAndGenerateTokenAsync(payload);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVm payload)
    {
       var jwtTokenHandler = new JwtSecurityTokenHandler();

       try
       {
           //Check Jwt token format
           var tokenInVerification=jwtTokenHandler.ValidateToken(payload.Token,_tokenValidationParameters,out var validatedToken);
       
           //check encryption algorithm
           if (validatedToken is JwtSecurityToken jwtSecurityToken)
           {
               var result= jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
               if (result==null) return null;
           
           }
           //check date
           var utcExpiryDate=long.Parse(tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
           var expiryDate=UnixTimeStampToDateTimeInUTC(utcExpiryDate);
           if (expiryDate < DateTime.UtcNow)throw new Exception("Expired Token");
           //check token in db
           var dbRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);
           if (dbRefreshToken == null) throw new Exception("Refresh Token not found");
        
           else
           {
               //check valid id
               var jti=tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
               //token expiration
               if(dbRefreshToken.JwtId!=jti) throw new Exception("Refresh Token dos not match");
               //revoked token
               if(dbRefreshToken.DateExpire<DateTime.UtcNow) throw new Exception("Refresh Token has expired");
               if(dbRefreshToken.IsRevoked) throw new Exception("Revoked Token");
           
           
               var dbUserData=await _userManager.FindByIdAsync(dbRefreshToken.UserId);
               var newTokenRespones= GenerateJwtTokenAsync(dbUserData,payload.RefreshToken);
               return await newTokenRespones;
           }

       }
       catch (SecurityTokenExpiredException)
       {
           var dbRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);

           var dbUserData=await _userManager.FindByIdAsync(dbRefreshToken.UserId);
           var newTokenRespones= GenerateJwtTokenAsync(dbUserData,payload.RefreshToken);
           return await newTokenRespones;
       }

      


    }

    private async Task<AuthResultVM> GenerateJwtTokenAsync(User user,string existingRefreshToken)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            
        };
        
        //add roles
        var userRoles= await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
            
        }
        
        var authSigninKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(5),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
        );
        
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = new RefreshToken();
        if (string.IsNullOrEmpty(existingRefreshToken))
        {
            refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()

            };
            
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
        
        var respone = new AuthResultVM()
        {
            Token = jwtToken,
            RefreshToken = (string.IsNullOrEmpty(existingRefreshToken)) ? refreshToken.Token : existingRefreshToken,
            ExpiresAt = token.ValidTo



        };
        return respone;
    }

    private DateTime UnixTimeStampToDateTimeInUTC(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
        return dateTimeVal;

    }
}