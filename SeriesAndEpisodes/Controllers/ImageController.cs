﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriesAndEpisodes.Services;
using SeriesAndEpisodes.Utils;
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

        [HttpGet("public/{id:length(24)}", Name = "GetImage")]
        [FileResultContentType("image/jpeg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<FileContentResult> DownloadImage(string id)
        {
            var bytes = await _fileService.DownloadImage(id);

            return File(bytes, "image/jpeg", id + ".jpg");
        }
    }
}
