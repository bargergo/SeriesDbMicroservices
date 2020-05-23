using System;
using System.ComponentModel.DataAnnotations;

namespace SeriesAndEpisodes.DTOs
{
    public class EpisodeDetail
    {
        [Required] public int Id { get; set; }

        [Required] [MinLength(2)] public string Title { get; set; }

        [Required] public string Description { get; set; }

        [Required] public DateTime FirstAired { get; set; }

        [Required] public DateTime LastUpdated { get; set; }
    }
}