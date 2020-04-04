using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Services
{
    public class SeriesService
    {
        private readonly IMongoCollection<Series> _collection;
        private readonly IGridFSBucket _gridFsBucket;

        public SeriesService(ISeriesDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _gridFsBucket = new GridFSBucket(database);
            _collection = database.GetCollection<Series>(settings.SeriesCollectionName);
        }

        public async Task<List<Series>> GetAsync() =>
            (await _collection.FindAsync(book => true)).ToList();

        public async Task<Series> GetAsync(string id) =>
            (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();

        public async Task<Series> CreateAsync(Series series)
        {
            await _collection.InsertOneAsync(series);
            return series;
        }

        public async Task UpdateAsync(string id, Series seriesIn) =>
            await _collection.ReplaceOneAsync(series => series.Id == id, seriesIn);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(series => series.Id == id);

        public async Task UploadImage(string id, IFormFile image)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            if (series.ImageId != null)
            {
                await _gridFsBucket.DeleteAsync(ObjectId.Parse(series.ImageId));
            }
            var imageId = await _gridFsBucket.UploadFromStreamAsync(image.FileName, image.OpenReadStream());
            series.ImageId = imageId.ToString();
            await _collection.ReplaceOneAsync(series => series.Id == id, series);
        }

        public async Task<byte[]> DownloadImage(string id)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            if (series == null || series?.ImageId == null)
                return null;
            var objectId = ObjectId.Parse(series.ImageId);
            return await _gridFsBucket.DownloadAsBytesAsync(objectId);
        }
    }
}