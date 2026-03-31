using BePopJwt.Business.Dtos.ArtistDtos;
using BePopJwt.Business.Services.ArtistServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController(IArtistService _artistService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _artistService.GetAllAsync();
            return albums.IsSuccess ? Ok(albums) : BadRequest(albums);
        }
        [HttpGet("WithAlbums")]
        public async Task<IActionResult> GetAlbumsWithArtist()
        {
            var albums = await _artistService.GetArtistsWithAlbumsAsync();
            return albums.IsSuccess ? Ok(albums) : BadRequest(albums);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateArtistDto createArtistDto)
        {
            var result = await _artistService.CreateAsync(createArtistDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateArtistDto updateArtistDto)
        {
            var result = await _artistService.UpdateAsync(updateArtistDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _artistService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _artistService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
