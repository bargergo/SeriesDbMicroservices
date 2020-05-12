using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.MessageQueue
{
    public class DummyMessage : IDummyMessage
    {
        public string DummyString { get; set; }
    }
}
