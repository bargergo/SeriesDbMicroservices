using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriesAndEpisodes.DTOs;
using SeriesAndEpisodes.Models;
using SeriesAndEpisodes.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Controllers
{
    [Route("Series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly SeriesService _seriesService;

        public SeriesController(SeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SeriesInfo>>> GetAsync() =>
            await _seriesService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetSeries")]
        public async Task<ActionResult<SeriesDetail>> GetAsync(string id)
        {
            var series = await _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            return series;
        }

        [HttpPost]
        public async Task<ActionResult<SeriesDetail>> CreateAsync(UpsertSeriesRequest series)
        {
            var createdSeries = await _seriesService.CreateAsync(series);

            return CreatedAtRoute("GetSeries", new { id = createdSeries.Id.ToString() }, createdSeries);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, UpsertSeriesRequest seriesIn)
        {
            var series = _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            await _seriesService.UpdateAsync(id, seriesIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var series = await _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            await _seriesService.RemoveAsync(series.Id);

            return NoContent();
        }

        [HttpPost("{id:length(24)}/image")]
        public async Task<IActionResult> UploadImage(string id, IFormFile image)
        {
            await _seriesService.UploadImage(id, image);
            return NoContent();
        }

        [HttpPost("{id:length(24)}/Season")]
        public async Task<IActionResult> AddSeason(string id, CreateSeasonRequest request)
        {
            await _seriesService.AddSeason(id, request);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}/Season/{seasonId:int}")]
        public async Task<IActionResult> DeleteSeason(string id, int seasonId)
        {
            await _seriesService.DeleteSeason(id, seasonId);
            return NoContent();
        }

        [HttpPost("{id:length(24)}/Season/{seasonId:int}")]
        public async Task<IActionResult> AddEpisode(string id, int seasonId, CreateEpisodeRequest request)
        {
            await _seriesService.AddEpisode(id, seasonId, request);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}/Season/{seasonId:int}/Episode/{episodeId:int}")]
        public async Task<IActionResult> AddEpisode(string id, int seasonId, int episodeId)
        {
            await _seriesService.DeleteEpisode(id, seasonId, episodeId);
            return NoContent();
        }
    }
}
