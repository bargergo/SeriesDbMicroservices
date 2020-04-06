using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Models
{
    public class Season
    {
        public int Id { get; set; }

        public List<Episode> Episodes { get; set; }
    }
}
