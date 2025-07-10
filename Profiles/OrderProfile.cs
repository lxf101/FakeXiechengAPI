using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Profiles
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                dest => dest.State, 
                opt => {
                    opt.MapFrom(src => src.State.ToString());
                });
        }
    }
}
