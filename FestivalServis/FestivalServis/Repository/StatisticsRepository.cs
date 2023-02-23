using FestivalServis.Interfaces;
using FestivalServis.Models;
using FestivalServis.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace FestivalServis.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly AppDbContext _context;

        public StatisticsRepository(AppDbContext context)
        {
            _context = context;
        }

        public StatisticsDTO GetStatistics()
        {
            return new StatisticsDTO() { PriceData = GetAveragePriceForFestivalsInPlace() };
        }

        private IEnumerable<PriceStatisticsDTO> GetAveragePriceForFestivalsInPlace()
        {
            return _context.Festivals.Include(f => f.Place).GroupBy(f => f.PlaceId)
                .Select(r => new PriceStatisticsDTO
                {
                    Place = _context.Places.Where(place => place.Id == r.Key).Select(place => place.Name).Single(),
                    AveragePrice = Math.Round(r.Average(r => r.TicketPrice),2)

                }).OrderByDescending(p => p.AveragePrice).ToList();
        }

       
    }
}

 