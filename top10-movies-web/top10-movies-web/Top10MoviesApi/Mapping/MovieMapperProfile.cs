using AutoMapper;
using Top10MoviesApi.DTO;
using Top10MoviesApi.Models;

namespace Top10MoviesApi.Mappings
{
    public class MovieMapperProfile : Profile
    {
        public MovieMapperProfile()
        {
            CreateMap<Movie, MovieDto>();
            CreateMap<CreateMovieDto, Movie>();
            CreateMap<UpdateMovieDto, Movie>();
        }
    }
}