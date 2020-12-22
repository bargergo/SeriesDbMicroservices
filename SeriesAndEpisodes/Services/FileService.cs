using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using SeriesAndEpisodes.Controllers;
using SeriesAndEpisodes.Exceptions;
using SeriesAndEpisodes.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Services
{
    public class FileService
    {
        private readonly IGridFSBucket _gridFsBucket;
        private readonly IFileSettings _fileSettings;
        private string[] permittedExtensions = { ".jpg" };

        public FileService(SeriesDbContext dbContext, IFileSettings fileSettings)
        {
            _gridFsBucket = dbContext.GetGridFSBucket();
            _fileSettings = fileSettings;
        }

        public async Task DeleteImage(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _gridFsBucket.DeleteAsync(objectId);
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                throw new BadRequestException("Not supported file extension");
            if (image.Length > _fileSettings.MaxSizeInMegabytes * 1024 * 1024)
                throw new BadRequestException("Size limit reached");

            var objectId = await _gridFsBucket.UploadFromStreamAsync(image.FileName, image.OpenReadStream());
            return objectId.ToString();
        }

        public async Task<byte[]> DownloadImage(string id)
        {
            var objectId = ObjectId.Parse(id);
            try
            {
                return await _gridFsBucket.DownloadAsBytesAsync(objectId);
            } catch(Exception)
            {
                throw new FileDoesntExistException();
            }
        }
    }
}
