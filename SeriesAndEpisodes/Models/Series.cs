using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SeriesAndEpisodes.Models
{
    public class Series
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime FirstAired { get; set; }

        public DateTime LastUpdated { get; set; }

        public List<Season> Seasons { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ImageId { get; set; }

        public float AverageRating { get; set; }

        public long NumberOfRatings { get; set; }
    }
}
