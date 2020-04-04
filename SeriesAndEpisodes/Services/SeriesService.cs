using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Services
{
    public class SeriesService
    {
        private readonly IMongoCollection<Series> _collection;

        public SeriesService(ISeriesDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<Series>(settings.SeriesCollectionName);
        }

        public async Task<List<Series>> GetAsync() =>
            (await _collection.FindAsync(book => true)).ToList();

        public async Task<Series> GetAsync(string id) =>
            (await _collection.FindAsync<Series>(series => series.Id == id)).FirstOrDefault();

        public async Task<Series> CreateAsync(Series series)
        {
            await _collection.InsertOneAsync(series);
            return series;
        }

        public async Task UpdateAsync(string id, Series seriesIn) =>
            await _collection.ReplaceOneAsync(series => series.Id == id, seriesIn);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(series => series.Id == id);
    }
}