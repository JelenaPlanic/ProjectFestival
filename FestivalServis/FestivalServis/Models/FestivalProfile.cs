using AutoMapper;
using FestivalServis.Models.DTO;

namespace FestivalServis.Models
{
    public class FestivalProfile : Profile
    {
        public FestivalProfile()
        {
            CreateMap<Festival, FestivalDTO>();
        }
    }
}
