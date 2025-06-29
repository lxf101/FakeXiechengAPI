using AutoMapper;
using FakeXiechengAPI.Dtos;
using FakeXiechengAPI.Models;
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

        [HttpGet("{pictureId}", Name = "GetPicture")]
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

        public IActionResult CreateTouristRoutePicture([FromRoute] Guid touristRouteId, [FromBody] TouristRoutePictureForCreationDto touristRoutePictureForCreationDto)
        {
            // 检测旅游路线是否存在
            if (!_touristRouteRepository.TouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            _touristRouteRepository.Save();

            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute("GetPicture", new { touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id}, pictureToReturn);
        }


    }
}
