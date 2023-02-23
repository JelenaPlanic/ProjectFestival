using FestivalServis.Models;

namespace FestivalServis.Interfaces
{
    public interface IPlaceRepository
    {
        IQueryable<Place> GetAll();
        Place GetById(int id);
        IQueryable<Place> GetByZipCode(int zipCode);


        //void Add(Place place);
        //void Update(Place place);   
        //void Delete(Place place);
    }
}
