using FestivalServis.Models.DTO;

namespace FestivalServis.Interfaces
{
    public interface IStatisticsRepository
    {
        StatisticsDTO GetStatistics();
    }
}
