using AutoMapper;
using FluentAssertions;
using MongoDB.Driver;
using SeriesAndEpisodes.AutoMapper;
using SeriesAndEpisodes.Models;
using SeriesAndEpisodes.Services;
using SeriesAndEpisodes.Tests.Fixtures;
using System;
using Xunit;

namespace SeriesAndEpisodes.Tests.Services
{
    public class SeriesServiceTest : IClassFixture<SeriesDbFixture>, IDisposable
    {
        private readonly SeriesDbFixture _fixture;
        private readonly SeriesService _service;

        public SeriesServiceTest(SeriesDbFixture fixture)
        {
            _fixture = fixture;
            var fileService = new FileService(_fixture.DbContext, _fixture.FileSettings);
            var config = new MapperConfiguration(opts =>
                opts.AddProfile(new WebApiProfile())
            );

            var mapper = config.CreateMapper();
            _service = new SeriesService(_fixture.DbContext, fileService, mapper);
        }

        public void Dispose()
        {
            _fixture.DbContext.GetCollection().DeleteMany(Builders<Series>.Filter.Empty);
        }

        [Fact(Timeout = 500)]
        public async void GetAsync_WithEmptyCollection_Returns0Series()
        {
            // Arrange

            // Act
            var series = await _service.GetAsync();

            // Assert
            series.Should().HaveCount(0);
        }

        [Fact(Timeout = 500)]
        public async void GetAsync_WithNotEmptyCollection_ReturnsAllSeries()
        {
            // Arrange
            var collection = _fixture.DbContext.GetCollection();
            await collection.InsertOneAsync(new Models.Series
            {
                Title = "Supernatural",
                Description = "Two brothers follow their father's footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.",
                FirstAired = new DateTime(2006, 07, 28, 22, 35, 5),
                LastUpdated = DateTime.Now
            });

            // Act
            var series = await _service.GetAsync();

            // Assert
            series.Should().HaveCount(1);
        }

        [Fact(Timeout = 500)]
        public async void CreateAsync_IntoNotEmptyCollection_Succeeds()
        {
            // Arrange
            var collection = _fixture.DbContext.GetCollection();
            await collection.InsertOneAsync(new Models.Series
            {
                Title = "Supernatural",
                Description = "Two brothers follow their father's footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.",
                FirstAired = new DateTime(2006, 07, 28, 22, 35, 5),
                LastUpdated = DateTime.Now
            });
            // Act
            await _service.CreateAsync(new DTOs.UpsertSeriesRequest { 
                Title = "Dummy Series",
                Description = "Best series ever",
                FirstAired = DateTime.UtcNow
            });

            // Assert
            var series = await _service.GetAsync();
            series.Should().HaveCount(2);
        }

        [Fact(Timeout = 500)]
        public async void RemoveAsync_FromNotEmptyCollection_Succeeds()
        {
            // Arrange
            var collection = _fixture.DbContext.GetCollection();
            var newSeries = new Models.Series
            {
                Title = "Supernatural",
                Description = "Two brothers follow their father's footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.",
                FirstAired = new DateTime(2006, 07, 28, 22, 35, 5),
                LastUpdated = DateTime.Now
            };
            await collection.InsertOneAsync(newSeries);
            // Act
            await _service.RemoveAsync(newSeries.Id);

            // Assert
            var series = await _service.GetAsync();
            series.Should().HaveCount(0);
        }
    }
}
