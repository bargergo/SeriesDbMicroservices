using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeriesInfo
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime FirstAired { get; set; }

        public DateTime LastUpdated { get; set; }

        public string ImageId { get; set; }
    }
}
