using AutoMapper;
using DiscountAPI.DTO;
using DiscountAPI.Models;

namespace DiscountAPI.Mapper;

public class DiscountProfile : Profile
{
  public DiscountProfile()
  {
    CreateMap<Discount, DiscountDTO>()
    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.discountId))
    .ReverseMap();
    CreateMap<DiscountProduct, DiscountProductDTO>().ReverseMap();
  }
}