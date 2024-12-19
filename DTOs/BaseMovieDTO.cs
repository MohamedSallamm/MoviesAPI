namespace MoviesAPI.DTOs
{
    public class BaseMovieDTO
    {
        [MaxLength(length: 200)]
        public string Title { get; set; }
        public int year { get; set; }
        public double Rate { get; set; }
        [MaxLength(length: 3000)]
        public string StoryLine { get; set; }
        public byte GenreId { get; set; }
    }
}
