using System;

namespace SeriesAndEpisodes.DTOs
{
    public class EpisodeDetail
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime FirstAired { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}