using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;

namespace FakeXiechengAPI.controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController: Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }
        
        // 获取用户订单
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders()
        {
            // 获取当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 使用用户id获取订单历史记录
            var orders = await _touristRouteRepository.GetOrdersByUserId(userId);
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        // 获取订单详情
        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> getOrderById([FromRoute] Guid orderId)
        {
            var order = await _touristRouteRepository.GetOrderById(orderId);
            return Ok(_mapper.Map<OrderDto>(order));
        }

        // 模拟支付
        //[HttpPost("{orderId}/placeOrder")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //public async Task<IActionResult> PlaceOrder([FromRoute] Guid orderId)
        //{
        //    // 1. 获得当前用户
        //    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    // 2. 开始处理支付
        //    var order = await _touristRouteRepository.GetOrderById(orderId);
        //    order.PaymentProcessing();
        //    await _touristRouteRepository.SaveAsync();
        //    // 3. 向第三方提交支付请求，等待第三方响应
        //    var httpClient = _httpClientFactory.CreateClient();
        //    string url = @"http://123.56.149.216/api/FakePaymentProcess?icode={0}&orderNumber={1}&returnFault={2}";
        //    var response = await httpClient.PostAsync(
        //        string.Format(url, "5302F6674825B162", order.Id, false),
        //        null
        //    );
        //    // 4. 提取支付结果，以及支付信息

        //    // 5. 如果第三方支付成功，完成订单

        //}
    }
}
