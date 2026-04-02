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
            {
                return BadRequest(new { ok = false, message = "Dosya boş olamaz." });
            }

            // 🔒 extension kontrol
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension is not ".mp3" and not ".wav" and not ".m4a")
            {
                return BadRequest(new { ok = false, message = "Sadece mp3/wav/m4a yüklenebilir." });
            }

            // 🔒 mime kontrol
            if (!file.ContentType.StartsWith("audio/"))
            {
                return BadRequest(new { ok = false, message = "Geçersiz dosya tipi." });
            }

            var uploadedUrl = await audioStorageService.UploadSongAsync(file);

            return Ok(new
            {
                ok = true,
                url = uploadedUrl
            });
        }
    }
}
