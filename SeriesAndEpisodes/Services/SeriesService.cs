using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;

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

        public List<Series> Get() =>
            _collection.Find(book => true).ToList();

        public Series Get(string id) =>
            _collection.Find<Series>(series => series.Id == id).FirstOrDefault();

        public Series Create(Series series)
        {
            _collection.InsertOne(series);
            return series;
        }

        public void Update(string id, Series seriesIn) =>
            _collection.ReplaceOne(series => series.Id == id, seriesIn);

        public void Remove(string id) =>
            _collection.DeleteOne(series => series.Id == id);
    }
}