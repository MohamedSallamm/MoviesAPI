namespace MoviesAPI.DTOs
{
    public class GenreDTO
    {
        [MaxLength(length:100)]
        public string Name { get; set; }
    }
}
