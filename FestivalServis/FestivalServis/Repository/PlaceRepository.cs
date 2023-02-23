using FestivalServis.Interfaces;
using FestivalServis.Models;

namespace FestivalServis.Repository
{
    public class PlaceRepository :IPlaceRepository
    {
        private readonly AppDbContext _context;

        public PlaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Place> GetAll()
        {
            return _context.Places;
        }

        public Place GetById(int id)
        {
            return _context.Places.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Place> GetByZipCode(int zipCode)
        {
            return _context.Places.Where(x => x.ZipCode < zipCode).OrderBy(x => x.ZipCode);
        }



    }
}
