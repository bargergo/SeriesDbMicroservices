using Microsoft.AspNetCore.Mvc;
using SeriesAndEpisodes.Models;
using SeriesAndEpisodes.Services;
using System;
using System.Collections.Generic;
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
        public ActionResult<List<Series>> Get() =>
            _seriesService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSeries")]
        public ActionResult<Series> Get(string id)
        {
            var series = _seriesService.Get(id);

            if (series == null)
            {
                return NotFound();
            }

            return series;
        }

        [HttpPost]
        public ActionResult<Series> Create(Series series)
        {
            _seriesService.Create(series);

            return CreatedAtRoute("GetSeries", new { id = series.Id.ToString() }, series);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Series seriesIn)
        {
            var series = _seriesService.Get(id);

            if (series == null)
            {
                return NotFound();
            }

            _seriesService.Update(id, seriesIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var series = _seriesService.Get(id);

            if (series == null)
            {
                return NotFound();
            }

            _seriesService.Remove(series.Id);

            return NoContent();
        }
    }
}
