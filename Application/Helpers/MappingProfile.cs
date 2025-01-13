using AutoMapper;

namespace Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Models.Product, Application.DTOs.Product.FullProductDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<Domain.Models.ProductDetail, Application.DTOs.Product.FullProductDTO>()
                .ForMember(dest => dest.DetailInformation, opt => opt.MapFrom(src => src.DetailInformation ?? new List<Domain.Models.DetailInformation>()));


        }
    }
}
