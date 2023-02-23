using System.ComponentModel.DataAnnotations;

namespace FestivalServis.Models
{
    public class Place
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5)]
        public int ZipCode { get; set; }
    }
}
