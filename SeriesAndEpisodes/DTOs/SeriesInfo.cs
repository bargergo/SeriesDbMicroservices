using System;
using System.ComponentModel.DataAnnotations;

namespace SeriesAndEpisodes.DTOs
{
    public class SeriesInfo
    {
        [Required] [RegularExpression("^[0-9a-fA-F]{24}$")]  public string Id { get; set; }

        [Required] public string Title { get; set; }

        [Required] public string Description { get; set; }

        [Required] public DateTime FirstAired { get; set; }

        [Required] public DateTime LastUpdated { get; set; }

        [Required] [RegularExpression("^[0-9a-fA-F]{24}$")] public string ImageId { get; set; }
    }
}
