using MassTransit;
using SeriesAndEpisodes.DTOs;
using SeriesAndEpisodes.MessageQueue;
using SeriesAndEpisodes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Services
{
    public class RatingsService
    {
        private readonly HttpClient _client;

        public RatingsService(HttpClient client, IRatingsServiceSettings settings)
        {
            client.BaseAddress = new Uri(settings.BaseAddress);
            _client = client;
        }

        public async Task<AverageOfRatings> GetSeriesRatingStatsForSeries(string seriesId)
        {
            var response = await _client.GetAsync($"/api/SeriesRatings/Series/{seriesId}/Average");
            
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<AverageOfRatings>(responseStream);
        }
    }
}
