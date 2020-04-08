﻿using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SeriesAndEpisodes.DTOs;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;
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

            var options = new FindOptions<Series>()
            {
                Projection = Builders<Series>.Projection
                    .Exclude(s => s.Seasons)
            };
            var series = await _collection.FindAsync(series => true, options);
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
                        Title = e.Title,
                        Description = e.Description,
                        FirstAired = e.FirstAired,
                        LastUpdated = e.LastUpdated
                    }).ToList(),
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
                        Title = e.Title,
                        Description = e.Description,
                        FirstAired = e.FirstAired,
                        LastUpdated = e.LastUpdated
                    }).ToList(),
                }).ToList(),
                ImageId = newSeries.ImageId,

            }; 
        }

        public async Task EditEpisode(string id, int seasonId, CreateEpisodeRequest request)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            var season = series.Seasons.FirstOrDefault(s => s.Id == seasonId);
            var episode = season.Episodes.FirstOrDefault(e => e.Id == request.Id);
            var updatedEpisode = new Episode
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                FirstAired = request.FirstAired,
                LastUpdated = DateTime.UtcNow
            };
            season.Episodes = season.Episodes.Select(e => e.Id == request.Id ? updatedEpisode : e).ToList();
            var filter = Builders<Series>.Filter.Eq(s => s.Id, id);
            var update = Builders<Series>.Update.Set(s => s.Seasons, series.Seasons);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<EpisodeDetail> GetEpisode(string id, int seasonId, int episodeId)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            var episode = series.Seasons.FirstOrDefault(s => s.Id == seasonId).Episodes.FirstOrDefault(e => e.Id == episodeId);
            if (episode == null)
                return null;
            return new EpisodeDetail
            {
                Id = episode.Id,
                Title = episode.Title,
                Description = episode.Description,
                FirstAired = episode.FirstAired,
                LastUpdated = episode.LastUpdated
            };

        }

        public async Task DeleteEpisode(string id, int seasonId, int episodeId)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            var updatedSeasons = series.Seasons.Select(s => {
                var episodes = s.Episodes.Where(e => s.Id != seasonId || e.Id != episodeId).Select(e => e).ToList();
                return new Season
                {
                    Id = s.Id,
                    Episodes = episodes
                };
            }).Where(s => s.Episodes.Count > 0).Select(s => s).ToList();
            var filter = Builders<Series>.Filter.Eq(s => s.Id, id);
            var update = Builders<Series>.Update.Set(s => s.Seasons, updatedSeasons);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task AddEpisode(string id, int seasonId, CreateEpisodeRequest request)
        {
            var series = (await _collection.FindAsync(series => series.Id == id)).FirstOrDefault();
            var season = series.Seasons.FirstOrDefault(s => s.Id == seasonId);
            if (season == null)
            {
                season = new Season
                {
                    Id = seasonId,
                    Episodes = new List<Episode>()
                };
                series.Seasons.Add(season);
            }

            var newEpisode = new Episode
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                FirstAired = request.FirstAired,
                LastUpdated = DateTime.UtcNow
            };
            season.Episodes.Add(newEpisode);
            
            var filter = Builders<Series>.Filter.Eq(s => s.Id, id);
            var update = Builders<Series>.Update.Set(s => s.Seasons, series.Seasons);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateAsync(string id, UpsertSeriesRequest seriesIn) {
            var filter = Builders<Series>.Filter.Eq(s => s.Id, id);
            var update = Builders<Series>.Update
                .Set(s => s.Title, seriesIn.Title)
                .Set(s => s.Description, seriesIn.Description)
                .Set(s => s.FirstAired, seriesIn.FirstAired)
                .Set(s => s.LastUpdated, DateTime.UtcNow);
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
            var filter = Builders<Series>.Filter.Eq(s => s.Id, id);
            var update = Builders<Series>.Update.Set(s => s.ImageId, imageId);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}