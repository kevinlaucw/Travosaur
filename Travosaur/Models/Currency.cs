using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travosaur.Models
{
    public class Currency
    {
        public int CurrencyID { get; set; }

        [StringLength(3)]
        [Column(TypeName = "nchar")]
        public string CurrencyCode { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}