using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travosaur.Models
{
    public class State
    {
        public int StateID { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string StateName { get; set; }

        [Required]
        public int CountryID { get; set; }

        public bool Active { get; set; }
    }
}