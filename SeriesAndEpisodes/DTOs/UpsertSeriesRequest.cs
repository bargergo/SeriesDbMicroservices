using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class UpsertSeriesRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime FirstAired { get; set; }
    }
}
