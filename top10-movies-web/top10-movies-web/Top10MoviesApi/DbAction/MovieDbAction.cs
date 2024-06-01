using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Top10MoviesApi.Data;
using Top10MoviesApi.Models;

namespace Top10MoviesApi.DbAction
{
    public class MovieDbAction : IMovieDbAction
    {
        private readonly ILogger<MovieDbAction> _logger;
        private readonly IDatabaseConfig _databaseConfig;

        public MovieDbAction(ILogger<MovieDbAction> logger, IDatabaseConfig databaseConfig)
        {
            _logger = logger;
            _databaseConfig = databaseConfig;

            _logger.LogDebug($"Init MovieDbAction");
        }

        private SqliteConnection Connection => new SqliteConnection(_databaseConfig.ConnectionString);

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            try
            {
                _logger.LogDebug("Entering DBA GetMoviesAsync()");

                using var connection = Connection;
                var movies = await connection.QueryAsync<Movie>("SELECT * FROM Movies");
                return movies;
            }
            catch(Exception e)
            {
                _logger.LogError($"Error In DBA GetMoviesAsync() => {e.Message}");
                return null;
            }
           
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            try
            {
                _logger.LogDebug($"Entering DBA GetMovieByIdAsync({id})");
                using var connection = Connection;
                var movie = await connection.QueryFirstOrDefaultAsync<Movie>("SELECT * FROM Movies WHERE Id = @Id", new { Id = id });
                return movie;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In DBA GetMovieByIdAsync({id}) => {e.Message}");
                return null;
            }
           
        }

        public async Task AddMovieAsync(Movie movie)
        {
            try
            {
                _logger.LogDebug($"Entering DBA AddMovieAsync({movie.Title})");
                using var connection = Connection;
                await connection.ExecuteAsync(
                    "INSERT INTO Movies (Title, Category, Ranking, ImageUrl) VALUES (@Title, @Category, @Ranking, @ImageUrl)",
                    movie);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In DBA AddMovieAsync({movie.Title}) => {e.Message}");
            }
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            try
            {
                _logger.LogDebug($"Entering DBA UpdateMovieAsync({movie.Id})");
                using var connection = Connection;
                await connection.ExecuteAsync(
                    "UPDATE Movies SET Title = @Title, Category = @Category, Ranking = @Ranking, ImageUrl = @ImageUrl WHERE Id = @Id",
                    movie);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In DBA UpdateMovieAsync({movie.Id}) => {e.Message}");
            }
        }

        public async Task DeleteMovieAsync(int id)
        {
           
            try
            {
                using var connection = Connection;
                await connection.ExecuteAsync("DELETE FROM Movies WHERE Id = @Id", new { Id = id });
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In DBA DeleteMovieAsync({id}) => {e.Message}");
            }
        }
    }
    public interface IMovieDbAction
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
