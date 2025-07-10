using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Profiles
{
    public class ShoppingCartProfile: Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();
        }
    }
}
