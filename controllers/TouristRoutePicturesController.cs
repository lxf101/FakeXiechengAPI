using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiechengAPI.controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController: Controller
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!_touristRouteRepository.TouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }
            var pictures = _touristRouteRepository.GetPicturesByTouristRouteId(touristRouteId);
            if(pictures == null || pictures.Count() <= 0)
            {
                return NotFound("图片不存在！");
            }
            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId}")]
        public IActionResult GetPicture(Guid touristRouteId, int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var picture = _touristRouteRepository.GetPicture(pictureId);
            if(picture == null)
            {
                return NotFound("旅游路线图片不存在");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }


    }
}
