using FakeXiechengAPI.Models;
using System.Threading.Tasks;

namespace FakeXiechengAPI.Services
{
    public interface ITouristRouteRepository
    {
        // 获取所有的旅游路线
        // 添加 Task 转变为异步操作
        Task<IEnumerable<TouristRoute>> GetAllTouristRoutesAsync(string keyword, string operatorType, int? ratingValue, int pageSize, int pageNumber);
        // 根据旅游路线的id，获取单个旅游路线
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);
        // 检测旅游路线是否存在
        Task<bool> TouristRouteExistAsync(Guid touristRouteId);
        // 根据路线id,来获取图片
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        // 根据pictureId获取TouristRoutePicture
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);
        Task<bool> SaveAsync();
        // 添加旅游路线
        void AddTouristRoute(TouristRoute touristRoute);
        // 添加旅游路线图片
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        // 删除旅游路线
        void DeleteTouristRoute(TouristRoute touristRoute);
        // 删除旅游路线图片
        void DeleteTouristRoutePicture(TouristRoutePicture picture);

        // 获取用户购物车
        Task<ShoppingCart> GetShoppingCartByUserId(string userId);

        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);

        Task<LineItem> GetShoppingCartItemByItemId(int lineItemId);

        void DeleteShoppingCartItem(LineItem lineItem);

        Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids);

        Task AddOrderAsync(Order order);

        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        Task<Order> GetOrderById(Guid orderId);
    }
}
