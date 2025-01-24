using AutoMapper;

namespace Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Application.DTOs.Product.ProductDTO, Domain.Models.Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductDetail, opt => opt.Ignore());
            CreateMap<Domain.Models.Product, Application.DTOs.Product.FullProductDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<Domain.Models.ProductDetail, Application.DTOs.Product.FullProductDTO>()
                .ForMember(dest => dest.DetailInformation, opt => opt.MapFrom(src => src.DetailInformation ?? new List<Domain.Models.DetailInformation>()));
            CreateMap<Domain.Models.StoreItem, Application.DTOs.StoreItemDtos.FullStoreItemDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            CreateMap<Domain.Models.Store, Application.DTOs.StoreDtos.StoreDto>()
                 .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));

        }
    }
}
