using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.AlbumDtos;
using BePopJwt.Business.Services.AlbumServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController(IAlbumService _albumService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _albumService.GetAllAsync();
            return albums.IsSuccess ? Ok(albums) : BadRequest(albums);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAlbumDto createAlbumDto)
        {
            var result = await _albumService.CreateAsync(createAlbumDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult>Update(UpdateAlbumDto updateAlbumDto)
        {
            var result=await _albumService.UpdateAsync(updateAlbumDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            var result=await _albumService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var result=await _albumService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}