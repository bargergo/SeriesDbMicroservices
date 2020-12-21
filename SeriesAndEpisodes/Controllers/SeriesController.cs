using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriesAndEpisodes.DTOs;
using SeriesAndEpisodes.MessageQueue;
using SeriesAndEpisodes.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Controllers
{
    [Route("api/public/Series")]
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

        [HttpGet(Name = "GetAllSeries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SeriesInfo>>> GetAll() =>
            await _seriesService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetSeries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SeriesDetail>> Get(string id)
        {
            var series = await _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            await _bus.Publish<IDummyMessage>(new DummyMessage
            {
                DummyString = id
            });

            return series;
        }

        [HttpGet("{id:length(24)}/Season/{seasonId:int}/Episode/{episodeId:int}", Name = "GetEpisode")]
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
