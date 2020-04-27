using Microsoft.AspNetCore.Mvc;
using SeriesAndEpisodes.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Controllers
{
    [Route("api/Images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly FileService _fileService;

        public ImageController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{id:length(24)}", Name = "GetImage")]
        public async Task<IActionResult> DownloadImage(string id)
        {
            var bytes = await _fileService.DownloadImage(id);

            if (bytes == null)
                return NotFound();

            return File(bytes, "image/jpeg");
        }
    }
}
