using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Travosaur.Models
{
    public class Tour
    {
        [Display(Name = "Tour ID")]
        public int TourID { get; set; }

        [Display(Name = "Tour Name")]
        [StringLength(100, MinimumLength = 3)]
        public string TourName { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(200)]
        public string ShortDescription { get; set; }

        [Display(Name = "Description")]
        [StringLength(4000)]
        public string Description { get; set; }

        [Display(Name = "Tour Highlights")]
        [StringLength(2000)]
        public string Highlight { get; set; }

        [Display(Name = "Tour Includes")]
        [StringLength(2000)]
        public string Include { get; set; }

        [Display(Name = "Tour Excludes")]
        [StringLength(2000)]
        public string Exclude { get; set; }

        [Display(Name = "Departure Info")]
        [StringLength(2000)]
        public string DepartureInfo { get; set; }

        [Display(Name = "Tour Code")]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string TourCode { get; set; }

        [Display(Name = "Tour Type")]
        public int? TourTypeID { get; set; }

        [Display(Name = "City")]
        public int CityID { get; set; }

        [Display(Name = "Currency")]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string Currency { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Display(Name = "Travel Period Start")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Travel Period End")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Created By")]
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string CreatedBy { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Updated By")]
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string LastUpdatedBy { get; set; }

        [Display(Name = "Duration Day")]
        public int DurationDay { get; set; }

        [Display(Name = "Duration Hour")]
        public byte DurationHour { get; set; }

        [Display(Name = "Image")]
        public byte[] ImageData { get; set; }

        [Display(Name = "Redirect URL")]
        [StringLength(500)]
        public string RedirectURL { get; set; }

        [Display(Name = "T & C")]
        [StringLength(4000)]
        public string Term { get; set; }

        public bool English { get; set; }

        public bool Chinese { get; set; }

        public bool Indian { get; set; }

        public bool Japanese { get; set; }

        public bool Korean { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public virtual City City { get; set; }
    }
}
