using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Models
{
    public class RatingsServiceSettings : IRatingsServiceSettings
    {
        public string BaseAddress { get; set; }
    }

    public interface IRatingsServiceSettings
    {
        string BaseAddress { get; set; }
    }
}
