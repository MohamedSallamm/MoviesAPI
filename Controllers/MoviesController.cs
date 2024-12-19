using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using MoviesAPI.Services;
using System.Linq;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;

        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
        {
            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Movies = await _moviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDetailsDTO>>(Movies);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var Movie = await _moviesService.GetById(id);

            if (Movie == null)
                return NotFound();
            var dto = _mapper.Map<MovieDetailsDTO>(Movie);
            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var Movie = _moviesService.GetAll(genreId);
            var data = _mapper.Map<IEnumerable<MovieDetailsDTO>>(Movie);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateMovieDTO dto)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var IsValidGenre = await _genresService.IsvalidGenre(dto.GenreId);
            if (!IsValidGenre)
                return BadRequest(error: "Invalid Genere Id");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();
            await _moviesService.Add(movie);
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] UpdateMovieDTO dto)
        {
            var Movie = await _moviesService.GetById(id);
            if (Movie == null)
                return NotFound(value: $"No movie was found by id: {id}");
            var IsValidGenre = await _genresService.IsvalidGenre(dto.GenreId);
            if (!IsValidGenre)
                return BadRequest(error: "Invalid Genere Id");
            if (dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest(error: "Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                Movie.Poster = dataStream.ToArray();
            }

            Movie.Title = dto.Title;
            Movie.GenreId = dto.GenreId;
            Movie.year = dto.year;
            Movie.StoryLine = dto.StoryLine;
            Movie.Rate = dto.Rate;

            _moviesService.Update(Movie);
            return Ok(Movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var Movie = await _moviesService.GetById(id);
            if (Movie == null)
                return NotFound(value: $"No Movie Was Found by Id: {id}");
            _moviesService.Delete(Movie);
            return Ok(Movie);
        }
    }
}
