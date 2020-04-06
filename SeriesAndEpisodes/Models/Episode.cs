using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Models
{
    public class Episode
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime FirstAired { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
