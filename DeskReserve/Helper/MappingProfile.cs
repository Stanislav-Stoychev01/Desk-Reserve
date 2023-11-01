using AutoMapper;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FloorDto, Floor>();
            CreateMap<Floor, FloorDto>();
        }
    }
}
