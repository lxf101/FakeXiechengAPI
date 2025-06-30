using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiechengAPI.ResourceParameters;
using FakeXiechengAPI.Models;

namespace FakeXiechengAPI.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : Controller
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        // 构造函数注入依赖
        // 将controller和interface联系了起来，即在controller中可以调用接口
        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTouristRoutes([FromQuery] TouristRouteResourceParameters parameters)
        {
            var routes = _touristRouteRepository.GetAllTouristRoutes(parameters.Keyword, parameters.RatingOperator, parameters.RatingValue);
            if(routes == null || routes.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);
            return Ok(touristRoutesDto);
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var route = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if(route == null)
            {
                return NotFound("没有找到相应的旅游路线");
            }

            // 在此实现Model --> DTO    从模型到DTO的映射
            //var touristRouteDto = new TouristRouteDto()
            //{
            //    Id = route.Id,
            //    Title = route.Title,
            //    Description = route.Description,
            //    Price = route.OriginalPrice * (decimal)(route.DiscountPresent ?? 1),
            //    CreateTime = route.CreateTime,
            //    UpdateTime = route.UpdateTime,
            //    Features = route.Features,
            //    Fees = route.Fees,
            //    Notes = route.Notes,
            //    Rating = route.Rating.ToString(),
            //    TravelDays = route.TravelDays.ToString(),
            //    TripType = route.TripType.ToString(),
            //    DepartureCity = route.DepartureCity.ToString()
            //};

            var touristRouteDto = _mapper.Map<TouristRouteDto>(route);
            return Ok(touristRouteDto);
        }



        [HttpPost]
        public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            // 新Dto与model的映射关系
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();

            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute("GetTouristRouteById", new {touristRouteId = touristRouteToReturn.Id}, touristRouteToReturn);
        }

        [HttpPut("{touristRouteId}")]
        public IActionResult UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            // 1. 检测旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            // 2. 根据旅游路线id，找到旅游路线
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            // 3. 更新旅游路线
            //    映射DTO
            //    更新DTO
            //    映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            _touristRouteRepository.Save();

            return NoContent();
        }



    }
}
