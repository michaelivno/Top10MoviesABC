using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Top10MoviesApi.DbAction;
using Top10MoviesApi.DTO;
using Top10MoviesApi.Models;

namespace Top10MoviesApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieDbAction _movieDbAction;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieDbAction> _logger;

        public MovieService(ILogger<MovieDbAction> logger, IMovieDbAction movieDbAction, IMapper mapper)
        {
            _logger = logger;
            _movieDbAction = movieDbAction;
            _mapper = mapper;

            _logger.LogDebug($"Init MovieService");
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesAsync()
        {
            _logger.LogDebug("Entering GetMoviesAsync");
            try
            {
                var movies = await _movieDbAction.GetMoviesAsync();
                return _mapper.Map<IEnumerable<MovieDto>>(movies);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In GetMoviesAsync() => {e.Message}");
                return null;
            }
        }


        public async Task<IEnumerable<MovieDto>> AddMovieAsync(CreateMovieDto createMovieDto)
        {
            _logger.LogDebug("Entering AddMovieAsync");
            try
            {
                var movie = _mapper.Map<Movie>(createMovieDto);

                var movies = await _movieDbAction.GetMoviesAsync();

                // Check if a movie with the same title and category already exists
                if (movies.Any(m => m.Title == movie.Title && m.Category == movie.Category))
                {
                    _logger.LogError("Error In AddMovieAsync() => A movie with the same title and category already exists.");
                    throw new InvalidOperationException("A movie with the same title and category already exists.");
                }

                if (movies.Count() < 10 || movie.Ranking > movies.OrderBy(m => m.Ranking).First().Ranking)
                {
                    if (movies.Count() >= 10)
                    {
                        var lowestRankedMovie = movies.OrderBy(m => m.Ranking).First();
                        await _movieDbAction.DeleteMovieAsync(lowestRankedMovie.Id);
                    }

                    await _movieDbAction.AddMovieAsync(movie);
                }
                else
                {
                    _logger.LogError("Error In AddMovieAsync() => New movie ranking is not high enough to be added to the top 10 list.");
                    return Enumerable.Empty<MovieDto>();
                }

                var updatedMovies = await _movieDbAction.GetMoviesAsync();
                return _mapper.Map<IEnumerable<MovieDto>>(updatedMovies);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error in AddMovieAsync");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In AddMovieAsync() => {e.Message}");
                throw new Exception("There was an error processing your request.");
            }
        }

        //public async Task<IEnumerable<MovieDto>> AddMovieAsync(CreateMovieDto createMovieDto)
        //{
        //    _logger.LogDebug("Entering AddMovieAsync");
        //    try
        //    {
        //        var movie = _mapper.Map<Movie>(createMovieDto);

        //        var movies = await _movieDbAction.GetMoviesAsync();

        //        if (movies.Any(m => m.Title == movie.Title && m.Category == movie.Category))
        //        {
        //            _logger.LogError("Error In AddMovieAsync() => A movie with the same title and category already exists.");
        //            throw new InvalidOperationException("A movie with the same title and category already exists.");
        //            return null;
        //        }

        //        if (movies.Count() < 10 || movie.Ranking > movies.OrderBy(m => m.Ranking).First().Ranking)
        //        {
        //            if (movies.Count() >= 10)
        //            {
        //                var lowestRankedMovie = movies.OrderBy(m => m.Ranking).First();
        //                await _movieDbAction.DeleteMovieAsync(lowestRankedMovie.Id);
        //            }

        //            await _movieDbAction.AddMovieAsync(movie);
        //        }
        //        else
        //        {
        //            _logger.LogError($"Error In AddMovieAsync() => New movie ranking is not high enough to be added to the top 10 list.");
        //            return Enumerable.Empty<MovieDto>();
        //        }

        //        var updatedMovies = await _movieDbAction.GetMoviesAsync();
        //        return _mapper.Map<IEnumerable<MovieDto>>(updatedMovies);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"Error In AddMovieAsync() => {e.Message}");
        //        return null;
        //    }
        //}

        public async Task<IEnumerable<MovieDto>> UpdateMovieAsync(int id, UpdateMovieDto updateMovieDto)
        {
            _logger.LogDebug("Entering UpdateMovieAsync");
            try
            {
                var movie = await _movieDbAction.GetMovieByIdAsync(id);
                if (movie == null)
                {
                    return null;
                }
                else if (id != movie.Id)
                {
                    return null;
                }
                else
                {
                    _mapper.Map(updateMovieDto, movie);
                    await _movieDbAction.UpdateMovieAsync(movie);

                    var updatedMovies = await _movieDbAction.GetMoviesAsync();
                    return _mapper.Map<IEnumerable<MovieDto>>(updatedMovies);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error In UpdateMovieAsync() => {e.Message}");
                return null;
            }
        }

    }
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetMoviesAsync();
        Task<IEnumerable<MovieDto>> AddMovieAsync(CreateMovieDto createMovieDto);
        Task<IEnumerable<MovieDto>> UpdateMovieAsync(int id, UpdateMovieDto updateMovieDto);
    }
}
