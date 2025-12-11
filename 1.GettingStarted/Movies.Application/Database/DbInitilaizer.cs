// using Dapper;
//
// namespace Movies.Application.Database;
//
// public class DbInitilaizer
// {
//     private readonly IDbConnectionFactory _dbConnectionFactory;
//
//     public DbInitilaizer(IDbConnectionFactory dbConnectionFactory)
//     {
//         _dbConnectionFactory = dbConnectionFactory;
//     }
//
//     public async Task InitializeAsync()
//     {
//         using var connection = await _dbConnectionFactory.CreateConnectionAsync();
//
//         // Movies table
//         await connection.ExecuteAsync("""
//                                   
//                                       
//                                           CREATE TABLE IF NOT EXISTS movies (
//                                               id UUID PRIMARY KEY,
//                                               title TEXT NOT NULL,
//                                               slug TEXT NOT NULL UNIQUE,
//                                               description TEXT NOT NULL,
//                                               year_of_release INTEGER NOT NULL,
//                                               run_time INTEGER NOT NULL,
//                                               age INTEGER NOT NULL,
//                                               castList TEXT[] NOT NULL,
//                                               genres TEXT[] NOT NULL,
//                                               rating REAL NOT NULL DEFAULT 0,
//                                               country TEXT NOT NULL,
//                                               director TEXT NOT NULL,
//                                               cover TEXT NOT NULL,
//                                               video_link TEXT NOT NULL
//                                           );
//                                       """);
//
//         // Unique index on slug
//         await connection.ExecuteAsync("""
//                                           CREATE UNIQUE INDEX IF NOT EXISTS movies_slug_idx
//                                           ON movies USING btree(slug);
//                                       """);
//
//     }
// }