﻿using AutoMapper;

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


        }
    }
}
