namespace MoviesAPI.DTOs
{
    public class UpdateMovieDTO : BaseMovieDTO
    {
        public IFormFile? Poster { get; set; }
    }
}
