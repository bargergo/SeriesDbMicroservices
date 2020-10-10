using System;
using System.ComponentModel.DataAnnotations;

namespace SeriesAndEpisodes.DTOs
{
    public class UpsertSeriesRequest
    {
        [Required] public string Title { get; set; }

        [Required] public string Description { get; set; }

        [Required] public DateTime FirstAired { get; set; }
    }
}
