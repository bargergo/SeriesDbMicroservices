using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeasonDetail
    {
        [Required] public int Id { get; set; }

        [Required] public List<EpisodeDetail> Episodes { get; set; }
    }
}
