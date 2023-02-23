using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FestivalServis.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Place> Places { get; set; }
        public DbSet<Festival> Festivals { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Place place1 = new Place() { Id = 1, Name = "Budapest", ZipCode = 1055 };
            Place place2 = new Place() { Id = 2, Name = "Novi Sad", ZipCode = 21000 };
            Place place3 = new Place() { Id = 3, Name = "Budva", ZipCode = 85313 };

            builder.Entity<Place>().HasData(place1, place2, place3);

            Festival firstFestival = new Festival() { Id = 1, Name = "Sziget", YearOfFirstEvent = 1990, TicketPrice = 150, PlaceId = 1 };

            Festival secondFestival = new Festival() { Id = 2, Name = "Exit", YearOfFirstEvent = 2000, TicketPrice = 60, PlaceId = 2 };
          
            Festival thirdFestival = new Festival() { Id = 3, Name = "Sea Dance", YearOfFirstEvent = 2014, TicketPrice = 30.5, PlaceId = 3 };

            builder.Entity<Festival>().HasData(firstFestival , secondFestival, thirdFestival);


            base.OnModelCreating(builder);
        }
    }
}
