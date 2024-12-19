 

using MoviesAPI.DTOs;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _GenresService;

        public GenresController(IGenresService GenresService)
        {
            _GenresService = GenresService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllASync()
        {
            var Genres = await _GenresService.GetAll();
            return Ok(Genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDTO dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _GenresService.Add(genre);
            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDTO dto)
        {
            var genre = await _GenresService.GetById(id);
            if (genre == null) 
            return NotFound($"There is no genre was found with id: {id}");
            genre.Name = dto.Name;
            _GenresService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _GenresService.GetById(id);
            if(genre == null)
                return NotFound($"There is no genre was found with id: {id}");
            _GenresService.Delete(genre);
            return Ok(genre);
        }
    }
}
