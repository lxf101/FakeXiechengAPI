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
        public async Task<IActionResult> GetPictureListForTouristRoute(Guid touristRouteId)
        {
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在！");
            }
            var pictures = await _touristRouteRepository.GetPicturesByTouristRouteIdAsync(touristRouteId);
            if(pictures == null || pictures.Count() <= 0)
            {
                return NotFound("图片不存在！");
            }
            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid touristRouteId, int pictureId)
        {
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var picture = await _touristRouteRepository.GetPictureAsync(pictureId);
            if(picture == null)
            {
                return NotFound("旅游路线图片不存在");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTouristRoutePicture([FromRoute] Guid touristRouteId, [FromBody] TouristRoutePictureForCreationDto touristRoutePictureForCreationDto)
        {
            // 检测旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            await _touristRouteRepository.SaveAsync();

            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute("GetPicture", new { touristRouteId = pictureModel.TouristRouteId, pictureId = pictureModel.Id}, pictureToReturn);
        }

        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePicture([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            // 检测旅游路线是否存在
            if (!await _touristRouteRepository.TouristRouteExistAsync(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var picture = await _touristRouteRepository.GetPictureAsync(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

    }
}
