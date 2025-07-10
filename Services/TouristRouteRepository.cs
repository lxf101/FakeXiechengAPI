using FakeXiechengAPI.Database;
using FakeXiechengAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeXiechengAPI.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        // 操作数据库，上下文关系对象
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if(touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if(touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if(touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.TouristRoutePictures.Remove(picture);
        }

        public async Task<IEnumerable<TouristRoute>> GetAllTouristRoutesAsync(string keyword, string operatorType, int? ratingValue, int pageSize, int pageNumber)
        {
            // include vs join    实现表连接
            // IQueryable<T> 是一个可以构造查询的接口，支持延迟执行。
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }

            if(ratingValue >= 0)
            {
                switch (operatorType)
                {
                    case "largerThan":
                        result = result.Where(t => t.Rating >= ratingValue);
                        break;
                    case "lessThan":
                        result = result.Where(t => t.Rating <= ratingValue);
                        break;
                    case "equalTo":
                    default:
                        result = result.Where(t => t.Rating == ratingValue);
                        break;
                }
            }

            // pagination
            // skip
            var skip = (pageNumber - 1) * pageSize;
            // 以pageSize为标准显示一定量的数据
            result = result.Take(pageSize);

            return await result.ToListAsync();
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return await _context.ShoppingCarts
                    .Include(s => s.User)
                    .Include(s => s.ShoppingCartItems)
                    .ThenInclude(li => li.TouristRoute)
                    .Where(s => s.UserId == userId)
                    .FirstOrDefaultAsync();
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t=>t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> TouristRouteExistAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public async Task<LineItem> GetShoppingCartItemByItemId(int lineItemId)
        {
            return await _context.LineItems.Where(li => li.Id == lineItemId).FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> ids)
        {
            return await _context.LineItems.Where(li => ids.Contains(li.Id)).ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {
            var result = _context.Orders.Where(o => o.UserId == userId);

            // pagination
            // skip
            //var skip = (pageNumber - 1) * pageSize;
            //// 以pageSize为标准显示一定量的数据
            //result = result.Take(pageSize);

            //return await result.ToListAsync();

            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.TouristRoute).Where(o => o.Id == orderId).FirstOrDefaultAsync();
        }
    }
}
