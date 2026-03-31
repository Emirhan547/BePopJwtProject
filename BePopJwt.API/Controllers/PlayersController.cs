using BePopJwt.Business.Dtos.PlayerDtos;
using BePopJwt.Business.Services.PlayerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayerController(IPlayerService playerService) : ControllerBase
    {
        [HttpGet("songs")]
        public async Task<IActionResult> GetAccessibleSongs()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await playerService.GetAccessibleSongsAsync(userId.Value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("state")]
        public async Task<IActionResult> GetState()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await playerService.GetPlayerStateAsync(userId.Value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("play")]
        public async Task<IActionResult> Play(PlaySongRequestDto dto)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await playerService.PlaySongAsync(userId.Value, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await playerService.GetMyHistoryAsync(userId.Value);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var userId) ? userId : null;
        }
    }
}