using BePopJwt.Business.Dtos.AlbumDtos;
using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.Business.Services.SongServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController(ISongService _songService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _songService.GetAllAsync();
            return albums.IsSuccess ? Ok(albums) : BadRequest(albums);
        }
        [HttpGet("WithAlbum")]
        public async Task<IActionResult> GetSongsWithAlbum()
        {
            var songs = await _songService.GetSongsWithAlbumAsync();
            return songs.IsSuccess ? Ok(songs) : BadRequest(songs);
        }

        [HttpGet("WithAlbum/{id}")]
        public async Task<IActionResult> GetSongWithAlbumById(int id)
        {
            var song = await _songService.GetSongWithAlbumByIdAsync(id);
            return song.IsSuccess ? Ok(song) : BadRequest(song);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSongDto createSongDto)
        {
            var result = await _songService.CreateAsync(createSongDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSongDto updateSongDto)
        {
            var result = await _songService.UpdateAsync(updateSongDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _songService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _songService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
