using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.MessageQueue
{
    public class SeriesRatingChangedEvent : ISeriesRatingChangedEvent
    {
        public string SeriesId { get; set; }

        public long Count { get; set; }

        public float Average { get; set; }
    }
}
