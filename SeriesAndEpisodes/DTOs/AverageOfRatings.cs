using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class AverageOfRatings
    {
        [Required] public float average { get; set; }
        [Required] public long count { get; set; }
    }
}
