using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.MessageQueue
{
    public interface ISeriesRatingChangedEvent
    {
        public string SeriesId { get; }
        public long Count { get; }
        public float Average { get; }
    }
}
