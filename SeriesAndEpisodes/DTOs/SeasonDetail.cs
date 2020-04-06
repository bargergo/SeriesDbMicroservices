﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.DTOs
{
    public class SeasonDetail
    {
        public int Id { get; set; }

        public List<EpisodeDetail> Episodes { get; set; }
    }
}
