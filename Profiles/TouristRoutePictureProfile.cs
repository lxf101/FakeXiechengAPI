using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Profiles
{
    public class TouristRoutePictureProfile: Profile
    {
        public TouristRoutePictureProfile()
        {
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();
        }
    }
}
