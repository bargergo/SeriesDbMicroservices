using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriesAndEpisodes.AutoMapper
{
    public class WebApiProfile : Profile
    {
        public WebApiProfile()
        {
            CreateMap<Models.Episode, DTOs.EpisodeDetail>().ReverseMap();
            CreateMap<Models.Season, DTOs.SeasonDetail>().ReverseMap();
            CreateMap<Models.Series, DTOs.SeriesDetail>().ReverseMap();
        }
            
    }
}
