using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.Models
{
    public class FileModel
    {
        public Stream Stream { get; set; }
        public string Name { get; set; }
    }
}
