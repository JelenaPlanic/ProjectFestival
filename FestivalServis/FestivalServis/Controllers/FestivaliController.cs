using AutoMapper;
using AutoMapper.QueryableExtensions;
using FestivalServis.Interfaces;
using FestivalServis.Models;
using FestivalServis.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalServis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FestivaliController : ControllerBase
    {
        private readonly IFestivalRepository _festivalRepository;
        private readonly IMapper _mapper;

        public FestivaliController(IFestivalRepository festivalRepository, IMapper mapper)
        {
            _festivalRepository = festivalRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetFestivals()
        {
            return Ok(_festivalRepository.GetAll().ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetFestival(int id)
        {
            var found = _festivalRepository.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FestivalDTO>(found));
        }

        [HttpPost]
        public IActionResult PostFestival(Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _festivalRepository.Add(festival);

            return CreatedAtAction("GetFestival", new { id = festival.Id }, festival);
        }

        [HttpPut("{id}")]
        public IActionResult PutFestival(int id, Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != festival.Id)
            {
                return BadRequest();
            }

            try
            {
                _festivalRepository.Update(festival);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<FestivalDTO>(festival));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFestival(int id)
        {
            var found = _festivalRepository.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            _festivalRepository.Delete(found);

            return NoContent();
        }

        [HttpPost]
        [Route("pretraga")]
        public IActionResult Search(FestivalFilter filter)
        {   
            if(filter.Start < 1950 || filter.End > 2018)
            {
                return BadRequest();
            }

            return Ok(_festivalRepository.GetAllByParameters(filter.Start, filter.End).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [HttpGet]
        [Route("trazi")]
        public IActionResult GetFestivalsByPrice(double cena)
        {
            if (cena < 1)
            {
                return BadRequest();
            }

            return Ok(_festivalRepository.GetAllByPrice(cena).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [HttpGet]
        [Route("lokacije")]
        public IActionResult GetFestivalsByPlace(int mesto)
        {
            if ( mesto < 1)
            {
                return BadRequest();
            }

            return Ok(_festivalRepository.GetAllByPlace(mesto).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [HttpGet]
        [Route("/api/location")]
        public IActionResult GetFestivalsByLocation(string location)
        {
            if (location == null)
            {
                return BadRequest();
            }

            return Ok(_festivalRepository.GetAllByLocation(location).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [HttpGet]
        [Route("/api/dva")]
        public IActionResult GetTwoWithMaxPrice()
        {
            return Ok(_festivalRepository.GetTwoWithMaxPrice().ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());
        }

       
    }
}
