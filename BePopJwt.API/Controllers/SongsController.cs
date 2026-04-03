using BePopJwt.Business.Dtos.AlbumDtos;
using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.Business.Services.SongServices;
using BePopJwt.Business.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController(ISongService _songService, IAudioStorageService audioStorageService) : ControllerBase
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
        [HttpPost("upload-audio")]
        public async Task<IActionResult> UploadAudio([FromForm] IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Dosya boş");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension is not ".mp3" and not ".wav" and not ".m4a")
                return BadRequest("Sadece mp3/wav/m4a");

            if (!file.ContentType.StartsWith("audio/"))
                return BadRequest("Geçersiz mime");

            var url = await audioStorageService.UploadFileAsync(file, "songs");

            return Ok(new { url });
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Dosya boş");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension is not ".jpg" and not ".jpeg" and not ".png")
                return BadRequest("Sadece jpg/jpeg/png");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("Geçersiz mime");

            var url = await audioStorageService.UploadFileAsync(file, "images");

            return Ok(new { url });
        }
    }
}
