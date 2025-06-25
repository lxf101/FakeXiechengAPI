using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;

/**
 * TouristRoute 映射文件
 * */
namespace FakeXiechengAPI.Profiles
{
    public class TouristRouteProfile: Profile
    {
        public TouristRouteProfile()
        {
            // 配置映射关系
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1)))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.ToString()))
                .ForMember(dest => dest.TravelDays, opt => opt.MapFrom(src => src.TravelDays.ToString()))
                .ForMember(dest => dest.TripType, opt => opt.MapFrom(src => src.TripType.ToString()))
                .ForMember(dest => dest.DepartureCity, opt => opt.MapFrom(src => src.DepartureCity.ToString()));
        }
    }
}
