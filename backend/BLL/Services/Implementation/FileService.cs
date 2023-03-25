using backend.BLL.Common.Consts;
using backend.BLL.Common.Exceptions;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class FileService : IFileService
    {
        public async Task<string> SaveFile(IFormFile file, string folder)
        {
            try
            {
                string fileName = Guid.NewGuid().ToString() + '.' + file.FileName.Split('.').Last();

                var tempPath = Path.Combine(folder, fileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileConstants.StaticFilesFolder, tempPath);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return fileName;

            }
            catch (Exception)
            {
                throw new CustomHttpException("Server couldn't save your file...");
            }
        }
    }
}
