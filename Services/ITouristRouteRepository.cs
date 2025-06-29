using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.Services
{
    public interface ITouristRouteRepository
    {
        // 获取所有的旅游路线
        IEnumerable<TouristRoute> GetAllTouristRoutes(string keyword, string operatorType, int? ratingValue);
        // 根据旅游路线的id，获取单个旅游路线
        TouristRoute GetTouristRoute(Guid touristRouteId);
        // 检测旅游路线是否存在
        bool TouristRouteExist(Guid touristRouteId);
        // 根据路线id,来获取图片
        IEnumerable<TouristRoutePicture> GetPicturesByTouristRouteId(Guid touristRouteId);
        // 根据pictureId获取TouristRoutePicture
        TouristRoutePicture GetPicture(int pictureId);
        // 添加旅游路线
        void AddTouristRoute(TouristRoute touristRoute);
        bool Save();
        // 添加旅游路线图片
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
    }
}
