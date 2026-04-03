using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.StorageServices
{
    public interface IAudioStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    }
}
