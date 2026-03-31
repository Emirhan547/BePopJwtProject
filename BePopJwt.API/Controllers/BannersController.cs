using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.BannerDtos;
using BePopJwt.Business.Services.BannerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController(IBannerService _bannerService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult>GetAll()
        {
            var banners=await _bannerService.GetAllAsync();
            return banners.IsSuccess ? Ok(banners) : BadRequest(banners);
        }
        [HttpPost]
        public async Task<IActionResult>Create(CreateBannerDto createBannerDto)
        {
            var banners = await _bannerService.CreateAsync(createBannerDto);
            return banners.IsSuccess ? Ok(banners) : BadRequest(banners);
        }
        [HttpPut]
        public async Task<IActionResult>Update(UpdateBannerDto updateBannerDto)
        {
            var banners=await _bannerService.UpdateAsync(updateBannerDto);
            return banners.IsSuccess ? Ok(banners) : BadRequest(banners);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            var banners=await _bannerService.DeleteAsync(id);
            return banners.IsSuccess ? Ok(banners) : BadRequest(banners);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetById(int id)
        {
            var banners=await _bannerService.GetByIdAsync(id);
            return banners.IsSuccess ? Ok(banners) : BadRequest(banners);
        }
    }
}
