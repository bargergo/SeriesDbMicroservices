using MongoDB.Driver;
using SeriesAndEpisodes.Models;
using System;

namespace SeriesAndEpisodes.Tests.Fixtures
{
    public class SeriesDbFixture : IDisposable
    {
        public ISeriesDbSettings DbContextSettings { get; }
        public IFileSettings FileSettings { get; }

        public SeriesDbFixture()
        {
            var connectionString = "mongodb://root:rootpassword@localhost:27017";
            var hostname = Environment.GetEnvironmentVariable("MONGODB_HOST");
            var port = Environment.GetEnvironmentVariable("MONGODB_PORT");
            if (hostname != null)
                connectionString = $"mongodb://{hostname}:{port}";
            var dbName = $"test_db_{Guid.NewGuid()}";
            DbContextSettings = new SeriesDbSettings
            {
                SeriesCollectionName = "Series",
                ConnectionString = connectionString,
                DatabaseName = dbName
            };
            FileSettings = new FileSettings
            {
                MaxSizeInMegabytes = 5
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
