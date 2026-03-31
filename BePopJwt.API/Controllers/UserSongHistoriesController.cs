using BePopJwt.Business.Dtos.SongDtos;
using BePopJwt.Business.Dtos.UserSongHistoryDtos;
using BePopJwt.Business.Services.UserSongHistoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSongHistoriesController(IUserSongHistoryService _userSongHistoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _userSongHistoryService.GetAllAsync();
            return albums.IsSuccess ? Ok(albums) : BadRequest(albums);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserSongHistoryDto create)
        {
            var result = await _userSongHistoryService.CreateAsync(create);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserSongHistoryDto create)
        {
            var result = await _userSongHistoryService.UpdateAsync(create);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userSongHistoryService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userSongHistoryService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
