using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeriesInfo
    {
        [Required] public string Id { get; set; }

        [Required] public string Title { get; set; }

        [Required] public string Description { get; set; }

        [Required] public DateTime FirstAired { get; set; }

        [Required] public DateTime LastUpdated { get; set; }

        [Required] public string ImageId { get; set; }
    }
}
