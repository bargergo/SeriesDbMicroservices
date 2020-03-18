using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Models
{
    public class SeriesDbSettings : ISeriesDbSettings
    {
        public string SeriesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISeriesDbSettings
    {
        string SeriesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
