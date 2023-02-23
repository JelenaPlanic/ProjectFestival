using FestivalServis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalServis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MestaController : ControllerBase
    {
        private readonly IPlaceRepository _placeRepository;

        public MestaController(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        [HttpGet]
        public IActionResult GetPlaces()
        {
            return Ok(_placeRepository.GetAll().ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetPlace(int id)
        {
            var found = _placeRepository.GetById(id);

            if(found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpGet]
        [Route("trazi")]
        public IActionResult GetPlacesByZipCode(int kod)
        {
            if(kod <= 0)
            {
                return BadRequest();
            }

            return Ok(_placeRepository.GetByZipCode(kod).ToList());
        }

    }
}
