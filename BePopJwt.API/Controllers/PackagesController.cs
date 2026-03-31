using BePopJwt.Business.Base;
using BePopJwt.Business.Dtos.PackageDtos;
using BePopJwt.Business.Services.PackageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController (IPackageService _service): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result=await _service.GetAllAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult>Create(CreatePackageDto packageDto)
        {
            var result=await _service.CreateAsync(packageDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePackageDto packageDto)
        {
            var result=await _service.UpdateAsync(packageDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result=await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) :BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result=await _service.DeleteAsync(id);
            return result.IsSuccess? Ok(result) : BadRequest(result) ;
        }
    }
}
