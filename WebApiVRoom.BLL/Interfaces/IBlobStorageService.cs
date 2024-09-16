﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IBlobStorageService
    {
        Task<bool> BlobExistsAsync(string fileName);
        Task DeleteFileAsync(string fileUrl);
        Task<BlobStorageDTO> DownloadFileAsync(string fileName);
        Task<IEnumerable<BlobStorageDTO>> ListBlobsAsync();
        Task<BlobStorageDTO> UploadFileAsync(Stream fileStream, string fileName);

    }

}
