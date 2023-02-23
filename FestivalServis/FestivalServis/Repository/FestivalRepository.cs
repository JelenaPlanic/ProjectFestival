using FestivalServis.Interfaces;
using FestivalServis.Models;
using Microsoft.EntityFrameworkCore;

namespace FestivalServis.Repository
{
    public class FestivalRepository :IFestivalRepository
    {
        private readonly AppDbContext _context;

        public FestivalRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Festival> GetAll()
        {
            return _context.Festivals.Include(f => f.Place).OrderByDescending(f => f.TicketPrice);
        }

        public Festival GetById(int id)
        {
            return _context.Festivals.Include(f => f.Place).FirstOrDefault(f => f.Id == id);
        }

        public void Add(Festival festival)
        {
            _context.Festivals.Add(festival);
            _context.SaveChanges();
        }

        public void Update(Festival festival)
        {
            _context.Entry(festival).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Festival festival)
        {
            _context.Festivals.Remove(festival);
            _context.SaveChanges();
        }

        public IQueryable<Festival> GetAllByParameters(int start, int end)
        {
            return _context.Festivals.Include(f => f.Place).Where(f => f.YearOfFirstEvent >= start && f.YearOfFirstEvent <= end).OrderBy(f => f.YearOfFirstEvent);
        }

        public IQueryable<Festival> GetAllByPrice(double price)
        {
            return _context.Festivals.Include(f => f.Place).Where(f => f.TicketPrice < price).OrderByDescending(f => f.Name);
        }

        public IQueryable<Festival> GetAllByPlace(int placeId)
        {
            return _context.Festivals.Include(f => f.Place).Where(f => f.PlaceId == placeId);
        }

        public IQueryable<Festival> GetTwoWithMaxPrice()
        {
            return _context.Festivals.OrderByDescending(x => x.TicketPrice).Take(2);
        }

        public IQueryable<Festival> GetAllByLocation(string location)
        {
            return _context.Festivals.Where(x => x.Place.Name.Contains(location));
        }


    }
}
