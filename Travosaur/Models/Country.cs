using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travosaur.Models
{
    public class Country
    {
        public int CountryID { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string CountryName { get; set; }

        public bool Active { get; set; }
    }
}