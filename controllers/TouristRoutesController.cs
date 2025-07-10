using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Text.RegularExpressions;
using FakeXiechengAPI.ResourceParameters;
using FakeXiechengAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;



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
        public async Task<IActionResult> GetTouristRoutes([FromQuery] TouristRouteResourceParameters parameters)
        {
            var routes = await _touristRouteRepository.GetAllTouristRoutesAsync(parameters.Keyword, parameters.RatingOperator, parameters.RatingValue, parameters.PageSize, parameters.PageNumber);
            if(routes == null || routes.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRoutesDto = _mapper.Map<IEnumerable<TouristRouteDto>>(routes);
            return Ok(touristRoutesDto);
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId)
        {
            var route = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]     // (Roles = "Admin")
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            // 新Dto与model的映射关系
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            Console.WriteLine(touristRouteModel);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();

            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute("GetTouristRouteById", new {touristRouteId = touristRouteToReturn.Id}, touristRouteToReturn);
        }

        [HttpPut("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]
        public async Task<IActionResult> UpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            // 1. 检测旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            // 2. 根据旅游路线id，找到旅游路线
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            // 3. 更新旅游路线
            //    映射DTO
            //    更新DTO
            //    映射model
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]
        public async Task<IActionResult> PartiallyUpdateTouristRoute([FromRoute] Guid touristRouteId, [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            // 检测旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent(); // 返回204
        }

        [HttpDelete("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            // 检测旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var route = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(route);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        //[HttpDelete("({touristIDs})")]
        //public IActionResult DeleteByIDs([FromRoute] IEnumerable<Guid> touristIDs)
        //{

        //}
    }
}
