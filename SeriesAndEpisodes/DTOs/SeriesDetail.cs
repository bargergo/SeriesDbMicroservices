using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeriesDetail : SeriesInfo
    {
        public List<SeasonDetail> Seasons { get; set; }
    }
}
