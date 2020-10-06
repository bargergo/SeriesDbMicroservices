﻿using AutoMapper;
using FluentAssertions;
using SeriesAndEpisodes.AutoMapper;
using SeriesAndEpisodes.Services;
using SeriesAndEpisodes.Tests.Fixtures;
using System;
using Xunit;

namespace SeriesAndEpisodes.Tests.Services
{
    public class SeriesServiceTest : IClassFixture<SeriesDbFixture>
    {
        private readonly SeriesDbFixture _fixture;
        private readonly SeriesService _service;

        public SeriesServiceTest(SeriesDbFixture fixture)
        {
            _fixture = fixture;
            var fileService = new FileService(_fixture.DbContext);
            var config = new MapperConfiguration(opts =>
                opts.AddProfile(new WebApiProfile())
            );

            var mapper = config.CreateMapper();
            _service = new SeriesService(_fixture.DbContext, fileService, mapper);
        }

        [Fact]
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
    }
}
