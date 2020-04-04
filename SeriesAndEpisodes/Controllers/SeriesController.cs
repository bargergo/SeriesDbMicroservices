using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<Series>>> GetAsync() =>
            await _seriesService.GetAsync();

        [HttpGet("{id:length(24)}", Name = "GetSeries")]
        public async Task<ActionResult<Series>> GetAsync(string id)
        {
            var series = await _seriesService.GetAsync(id);

            if (series == null)
            {
                return NotFound();
            }

            return series;
        }

        [HttpPost]
        public async Task<ActionResult<Series>> CreateAsync(Series series)
        {
            await _seriesService.CreateAsync(series);

            return CreatedAtRoute("GetSeries", new { id = series.Id.ToString() }, series);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, Series seriesIn)
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

        [HttpGet("{id:length(24)}/image")]
        public async Task<IActionResult> DownloadImage(string id)
        {
            var stream = await _seriesService.DownloadImage(id);

            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", "image.jpg");
        }
    }
}
