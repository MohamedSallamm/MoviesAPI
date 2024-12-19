using AutoMapper;
using MoviesAPI.DTOs;

namespace MoviesAPI.Helper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDTO>();
            CreateMap<BaseMovieDTO, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());


        }
    }
}
