﻿using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriesAndEpisodes.DTOs;
using SeriesAndEpisodes.MessageQueue;
using SeriesAndEpisodes.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Controllers
{
    [Route("api/Series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly SeriesService _seriesService;
        private readonly IBusControl _bus;

        public SeriesController(SeriesService seriesService, IBusControl bus)
        {
            _seriesService = seriesService;
            _bus = bus;
        }

        [HttpGet("public", Name = "GetAllSeries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SeriesInfo>>> GetAll() =>
            await _seriesService.GetAsync();

        [HttpGet("public/{id:length(24)}", Name = "GetSeries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SeriesDetail>> Get(string id)
        {
            var series = await _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            await _bus.Publish(new DummyMessage
            {
                DummyString = id
            });

            return series;
        }

        [HttpPost("admin", Name = "CreateSeries")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SeriesDetail>> Create(UpsertSeriesRequest series)
        {
            var createdSeries = await _seriesService.CreateAsync(series);

            return CreatedAtRoute("GetSeries", new { id = createdSeries.Id.ToString() }, createdSeries);
        }


        [HttpPut("admin/{id:length(24)}", Name = "UpdateSeries")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, UpsertSeriesRequest seriesIn)
        {
            var series = _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            await _seriesService.UpdateAsync(id, seriesIn);

            return NoContent();
        }

        [HttpDelete("admin/{id:length(24)}", Name = "DeleteSeries")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPost("admin/{id:length(24)}/image", Name = "UploadImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(string id, IFormFile image)
        {
            await _seriesService.UploadImage(id, image);
            return NoContent();
        }

        [HttpPost("admin/{id:length(24)}/Season/{seasonId:int}", Name = "AddEpisode")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddEpisode(string id, int seasonId, CreateEpisodeRequest request)
        {
            var episode = await _seriesService.GetEpisode(id, seasonId, request.Id);
            if (episode != null)
            {
                return Conflict();
            }
            await _seriesService.AddEpisode(id, seasonId, request);
            return NoContent();
        }

        [HttpDelete("admin/{id:length(24)}/Season/{seasonId:int}/Episode/{episodeId:int}", Name = "DeleteEpisode")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteEpisode(string id, int seasonId, int episodeId)
        {
            await _seriesService.DeleteEpisode(id, seasonId, episodeId);
            return NoContent();
        }

        [HttpPut("admin/{id:length(24)}/Season/{seasonId:int}/Episode/{episodeId:int}", Name = "UpdateEpisode")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> EditEpisode(string id, int seasonId, int episodeId, CreateEpisodeRequest request)
        {
            var episodeToUpdate = await _seriesService.GetEpisode(id, seasonId, episodeId);
            if (episodeToUpdate == null)
            {
                return NotFound();
            }
            if (episodeId != request.Id)
            {
                var episodeWithTheNewId = await _seriesService.GetEpisode(id, seasonId, request.Id);
                if (episodeWithTheNewId != null)
                {
                    return Conflict();
                }
            }
            await _seriesService.EditEpisode(id, seasonId, episodeId, request);
            return NoContent();
        }

        [HttpGet("public/{id:length(24)}/Season/{seasonId:int}/Episode/{episodeId:int}", Name = "GetEpisode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EpisodeDetail>> GetEpisode(string id, int seasonId, int episodeId)
        {
            var episode = await _seriesService.GetEpisode(id, seasonId, episodeId);
            if (episode == null)
            {
                return NotFound();
            }

            return episode;
        }
    }
}
