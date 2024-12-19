namespace MoviesAPI.DTOs
{
    public class CreateMovieDTO : BaseMovieDTO
    {
        public IFormFile Poster { get; set; }
    }
}
