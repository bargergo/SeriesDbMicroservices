using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Services
{
    public class FileService
    {
        private readonly IGridFSBucket _gridFsBucket;

        public FileService(ISeriesDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _gridFsBucket = new GridFSBucket(database);
        }

        public async Task DeleteImage(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _gridFsBucket.DeleteAsync(objectId);
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            var objectId = await _gridFsBucket.UploadFromStreamAsync(image.FileName, image.OpenReadStream());
            return objectId.ToString();
        }

        public async Task<byte[]> DownloadImage(string id)
        {
            var objectId = ObjectId.Parse(id);
            return await _gridFsBucket.DownloadAsBytesAsync(objectId);
        }
    }
}
