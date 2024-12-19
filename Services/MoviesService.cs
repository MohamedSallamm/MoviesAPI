
namespace MoviesAPI.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
           _context.Remove(movie);
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await _context.Movies
                 .Where(m => m.GenreId == genreId || genreId == 0)
                 .OrderBy(m => m.Rate)
                 .ToListAsync();
        }

        public Task<Movie> GetById(int id)
        {
            return _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

        }

        public Movie Update(Movie movie)
        {
            _context.Remove(movie);
            return movie;
        }
    }
}
