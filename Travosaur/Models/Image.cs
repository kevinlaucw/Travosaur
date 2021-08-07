using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Travosaur.Models
{
    public class Image
    {
        public int ImageID { get; set; }

        [StringLength(250)]
        public string ImageName { get; set; }

        public int? ImageSize { get; set; }
        public byte[] ImageData { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select a file")]
        public HttpPostedFileBase File { get; set; }
    }
}
