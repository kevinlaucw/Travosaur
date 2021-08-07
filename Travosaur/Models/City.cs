using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travosaur.Models
{
    public class City
    {
        public int CityID { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string CityName { get; set; }

        [Required]
        public int StateID { get; set; }

        [NotMapped]
        public string StateName { get; set; }

        [Required]
        public int CountryID { get; set; }

        [NotMapped]
        public string CountryName { get; set; }

        public bool Active { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
    }
}