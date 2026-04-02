using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Services.StorageServices
{
    public interface IAudioStorageService
    {
        Task<string> UploadSongAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}
