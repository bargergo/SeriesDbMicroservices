using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SeriesAndEpisodes.DTOs;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace SeriesAndEpisodes.Services
{
    public class SeriesService
    {
        private readonly IMongoCollection<Series> _collection;
        private readonly FileService _fileService;

        public SeriesService(ISeriesDbSettings settings, FileService fileService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Series>(settings.SeriesCollectionName);
            _fileService = fileService;
        }

        public async Task<List<SeriesInfo>> GetAsync() {
            var series = await _collection.FindAsync(series => true);
            return series.ToList().Select(s => new SeriesInfo
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                FirstAired = s.FirstAired,
                LastUpdated = s.LastUpdated,
                ImageId = s.ImageId
            }).ToList();
        }


        public async Task<SeriesDetail> GetAsync(string id) {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            return new SeriesDetail
            {
                Id = series.Id,
                Title = series.Title,
                Description = series.Description,
                FirstAired = series.FirstAired,
                LastUpdated = series.LastUpdated,
                Seasons = series.Seasons.Select(s => new SeasonDetail
                {
                    Id = s.Id,
                    Episodes = s.Episodes.Select(e => new EpisodeDetail
                    {
                        Id = e.Id,
                        Number = e.Number,
                        Title = e.Title,
                        Description = e.Description,
                        FirstAired = e.FirstAired,
                        LastUpdated = e.LastUpdated
                    }).ToList(),
                    Number = s.Number
                }).ToList(),
                ImageId = series.ImageId
            };
        }

        public async Task<SeriesDetail> CreateAsync(UpsertSeriesRequest series)
        {
            var newSeries = new Series
            {
                Title = series.Title,
                Description = series.Description,
                FirstAired = series.FirstAired,
                LastUpdated = DateTime.UtcNow,
                Seasons = new List<Season>()
            };
            await _collection.InsertOneAsync(newSeries);
            return new SeriesDetail
            {
                Id = newSeries.Id,
                Title = newSeries.Title,
                Description = newSeries.Description,
                FirstAired = newSeries.FirstAired,
                LastUpdated = newSeries.LastUpdated,
                Seasons = newSeries.Seasons.Select(s => new SeasonDetail 
                {
                    Id = s.Id,
                    Episodes = s.Episodes.Select(e => new EpisodeDetail
                    {
                        Id = e.Id,
                        Number = e.Number,
                        Title = e.Title,
                        Description = e.Description,
                        FirstAired = e.FirstAired,
                        LastUpdated = e.LastUpdated
                    }).ToList(),
                    Number = s.Number
                }).ToList(),
                ImageId = newSeries.ImageId,

            }; 
        }

        public async Task UpdateAsync(string id, UpsertSeriesRequest seriesIn) {
            var filter = Builders<Series>.Filter.Eq("Id", id);
            var update = Builders<Series>.Update
                .Set("Title", seriesIn.Title)
                .Set("Description", seriesIn.Description)
                .Set("FirstAired", seriesIn.FirstAired)
                .Set("LastUpdated", DateTime.UtcNow);
            await _collection.UpdateOneAsync(filter, update);
        }


        public async Task RemoveAsync(string id)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            if (series.ImageId != null)
            {
                await _fileService.DeleteImage(series.ImageId);
            }
            await _collection.DeleteOneAsync(series => series.Id == id);
        }

        public async Task UploadImage(string id, IFormFile image)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            if (series.ImageId != null)
            {
                await _fileService.DeleteImage(series.ImageId);
            }
            var imageId = await _fileService.UploadImage(image);
            var filter = Builders<Series>.Filter.Eq("Id", id);
            var update = Builders<Series>.Update.Set("ImageId", imageId);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}