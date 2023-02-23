using FestivalServis.Models;

namespace FestivalServis.Interfaces
{
    public interface IFestivalRepository
    {
        IQueryable<Festival> GetAll();
        Festival GetById(int id);
        void Add(Festival festival);
        void Update(Festival festival);
        void Delete(Festival festival);

        IQueryable<Festival> GetAllByParameters(int start, int end);
        IQueryable<Festival> GetAllByPrice(double price);
        IQueryable<Festival> GetAllByPlace(int placeId);
        IQueryable<Festival> GetAllByLocation(string location);
        IQueryable<Festival> GetTwoWithMaxPrice();
       

    }
}
