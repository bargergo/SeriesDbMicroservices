using MongoDB.Driver;
using SeriesAndEpisodes.Models;
using System;

namespace SeriesAndEpisodes.Tests.Fixtures
{
    public class SeriesDbFixture : IDisposable
    {
        public ISeriesDbSettings DbContextSettings { get; }

        public SeriesDbFixture()
        {
            var dbName = $"test_db_{Guid.NewGuid()}";
            DbContextSettings = new SeriesDbSettings
            {
                SeriesCollectionName = "Series",
                ConnectionString = "mongodb://root:rootpassword@localhost:27017",
                DatabaseName = dbName
            };
            DbContext = new SeriesDbContext(DbContextSettings);
        }

        public SeriesDbContext DbContext { get; }

        public void Dispose()
        {
            var client = new MongoClient(this.DbContextSettings.ConnectionString);
            client.DropDatabase(this.DbContextSettings.DatabaseName);
        }
    }
}
