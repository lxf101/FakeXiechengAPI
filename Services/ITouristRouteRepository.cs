using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Services
{
    public interface ITouristRouteRepository
    {
        // 获取所有的旅游路线
        IEnumerable<TouristRoute> GetAllTouristRoutes(string keyword);
        // 根据旅游路线的id，获取单个旅游路线
        TouristRoute GetTouristRoute(Guid touristRouteId);
        // 检测旅游路线是否存在
        bool TouristRouteExist(Guid touristRouteId);
        // 根据路线id,来获取图片
        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);
        // 根据pictureId获取TouristRoutePicture
        TouristRoutePicture GetPicture(int pictureId);
    }
}
