using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeasonDetail
    {
        public string Id { get; set; }

        public List<EpisodeDetail> Episodes { get; set; }

        public int Number { get; set; }
    }
}
