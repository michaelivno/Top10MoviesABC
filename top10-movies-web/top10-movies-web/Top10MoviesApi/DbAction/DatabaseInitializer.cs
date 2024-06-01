using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Top10MoviesApi.Data;

namespace Top10MoviesApi
{
    public class DatabaseInitializer
    {
        private readonly IDatabaseConfig _databaseConfig;

        public DatabaseInitializer(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();

            // Create the Movies table if it doesn't exist
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Movies (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Ranking INTEGER NOT NULL,
                    ImageUrl TEXT
                );
            ";
            await command.ExecuteNonQueryAsync();

            // Check if there is any data in the Movies table
            command.CommandText = "SELECT COUNT(1) FROM Movies";
            var count = (long)await command.ExecuteScalarAsync();

            if (count > 0)
            {
                // Delete existing data
                command.CommandText = "DELETE FROM Movies";
                await command.ExecuteNonQueryAsync();
            }

            // Insert initial data
            command.CommandText = @"
                INSERT INTO Movies (Id, Title, Category, Ranking, ImageUrl) VALUES
                (1, 'Inception', 'Sci-Fi', 1, 'https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg'),
                (2, 'The Dark Knight', 'Action', 2, 'https://upload.wikimedia.org/wikipedia/en/1/1c/The_Dark_Knight_%282008_film%29.jpg'),
                (3, 'Forrest Gump', 'Drama', 3, 'https://upload.wikimedia.org/wikipedia/en/6/67/Forrest_Gump_poster.jpg'),
                (4, 'Pulp Fiction', 'Thriller', 4, 'https://upload.wikimedia.org/wikipedia/en/3/3b/Pulp_Fiction_%281994%29_poster.jpg'),
                (5, 'The Godfather', 'Drama', 5, 'https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg'),
                (6, 'The Shawshank Redemption', 'Drama', 6, 'https://m.media-amazon.com/images/I/51zUbui+gbL._AC_.jpg'),
                (7, 'The Matrix', 'Sci-Fi', 7, 'https://m.media-amazon.com/images/I/51EG732BV3L._AC_.jpg'),
                (8, 'Gladiator', 'Action', 8, 'https://upload.wikimedia.org/wikipedia/en/f/fb/Gladiator_%282000_film_poster%29.png'),
                (9, 'Fight Club', 'Thriller', 9, 'https://m.media-amazon.com/images/I/51v5ZpFyaFL._AC_.jpg'),
                (10, 'Interstellar', 'Sci-Fi', 10, 'https://m.media-amazon.com/images/I/91kFYg4fX3L._AC_SL1500_.jpg');
            ";
            await command.ExecuteNonQueryAsync();
        }
    }
}
