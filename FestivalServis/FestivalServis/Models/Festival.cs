using System.ComponentModel.DataAnnotations;

namespace FestivalServis.Models
{
    public class Festival
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double TicketPrice { get; set; }

        [Required]
        [Range(1951, 2017)]
        public int YearOfFirstEvent { get; set; }

        public Place? Place { get; set; }
        public int PlaceId { get; set; }
    }
}
