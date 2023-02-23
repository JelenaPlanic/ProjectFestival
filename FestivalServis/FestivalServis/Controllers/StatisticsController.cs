using FestivalServis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalServis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("/api/prosek")]
        public IActionResult GetStatistics()
        {
            return Ok(_statisticsRepository.GetStatistics());
        }
    }
}
