using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeriesDetail : SeriesInfo
    {
        [Required] public List<SeasonDetail> Seasons { get; set; }
        [Required] public float AverageRating { get; set; }
        [Required] public long NumberOfRatings { get; set; }
    }
}
