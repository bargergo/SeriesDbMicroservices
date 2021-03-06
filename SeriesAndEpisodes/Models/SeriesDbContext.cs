﻿using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Models
{
    public class SeriesDbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        private string _collectionName { get; set; }

        public SeriesDbContext(ISeriesDbSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _db = _mongoClient.GetDatabase(settings.DatabaseName);
            _collectionName = settings.SeriesCollectionName;
        }

        public async Task TestConnection()
        {
            await _mongoClient.ListDatabaseNamesAsync();
        }

        public IMongoCollection<Series> GetCollection()
        {
            return _db.GetCollection<Series>(_collectionName);
        }

        public GridFSBucket GetGridFSBucket()
        {
            return new GridFSBucket(_db);
        }
    }
}
