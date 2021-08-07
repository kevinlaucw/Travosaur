using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Travosaur.Models
{
    [Table("vw_Tour")]
    public class TourView
    {
        [Key]
        public int TourID { get; set; }

        public string TourName { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Highlight { get; set; }

        public string Include { get; set; }

        public string Exclude { get; set; }

        public string DepartureInfo { get; set; }

        public string TourCode { get; set; }

        public int? TourTypeID { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        public int? StateID { get; set; }

        public string StateName { get; set; }

        public int? CountryID { get; set; }

        public string CountryName { get; set; }

        public string Currency { get; set; }

        public decimal Price { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? DateCreated { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedByDisplayName { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public int DurationDay { get; set; }

        public byte DurationHour { get; set; }

        public byte[] ImageData { get; set; }
        //public System.Data.Linq.Binary ImageData { get; set; }

        public string RedirectURL { get; set; }

        public string Term { get; set; }

        public bool English { get; set; }

        public bool Chinese { get; set; }

        public bool Indian { get; set; }

        public bool Japanese { get; set; }

        public bool Korean { get; set; }
    }

    public class TourViewModel
    {
        public int TourID { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Tour Name")]
        public string TourName { get; set; }

        [StringLength(200)]
        [Display(Name = "Short Desc")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [AllowHtml]
        [Display(Name = "Tour Highlights")]
        [StringLength(2000)]
        public string Highlight { get; set; }

        [AllowHtml]
        [Display(Name = "Tour Includes")]
        [StringLength(2000)]
        public string Include { get; set; }

        [AllowHtml]
        [Display(Name = "Tour Excludes")]
        [StringLength(2000)]
        public string Exclude { get; set; }

        [AllowHtml]
        [Display(Name = "Departure Info")]
        [StringLength(2000)]
        public string DepartureInfo { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Tour Code")]
        public string TourCode { get; set; }

        public int? TourTypeID { get; set; }

        public int CityID { get; set; }

        //[NotMapped]
        [Display(Name = "City")]
        public string CityName { get; set; }

        //[NotMapped]
        [Display(Name = "State")]
        public string StateName { get; set; }

        //[NotMapped]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [StringLength(3)]
        [Column(TypeName = "nchar")]
        public string Currency { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(NullDisplayText = "0", DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }

        //[NotMapped]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(NullDisplayText = "0", DataFormatString = "{0:N0}")]
        public decimal RoundedPrice { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        //[NotMapped]
        public string CreatedByName { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }

        [Range(0, 100)]
        [Display(Name = "Duration Day")]
        public int DurationDay { get; set; }

        [Range(0, 24)]
        [Display(Name = "Duration Hour")]
        public byte DurationHour { get; set; }

        public byte[] ImageData { get; set; }

        //[NotMapped]
        [Display(Name = "Image")]
        public HttpPostedFileBase File { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        [StringLength(500)]
        public string RedirectURL { get; set; }

        [StringLength(4000)]
        public string Term { get; set; }

        public bool English { get; set; }

        [Display(Name = "中文")]
        public bool Chinese { get; set; }

        [Display(Name = "हिंदी")]
        public bool Indian { get; set; }

        [Display(Name = "한국어")]
        public bool Japanese { get; set; }

        [Display(Name = "日本語")]
        public bool Korean { get; set; }
    }

    public class ManageTourViewModel
    {
        ApplicationDbContext db = null;

        public ManageTourViewModel()
        {
            db = new ApplicationDbContext();
        }

        public int TourID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Tour Name")]
        public string TourName { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Short Desc")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [AllowHtml]
        [Display(Name = "Tour Highlights")]
        [StringLength(2000)]
        public string Highlight { get; set; }

        [AllowHtml]
        [Display(Name = "Tour Includes")]
        [StringLength(2000)]
        public string Include { get; set; }

        [AllowHtml]
        [Display(Name = "Tour Excludes")]
        [StringLength(2000)]
        public string Exclude { get; set; }

        [AllowHtml]
        [Display(Name = "Departure Info")]
        [StringLength(2000)]
        public string DepartureInfo { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Tour Code")]
        public string TourCode { get; set; }

        public int? TourTypeID { get; set; }

        [Display(Name = "Tour Location")]
        public int CityID { get; set; }

        public List<City> Cities { get; set; }

        [Required(ErrorMessage = "Please select a currency")]
        [StringLength(3)]
        [Column(TypeName = "nchar")]
        public string Currency { get; set; }

        public IEnumerable<Currency> Currencies { get; set; }

        public bool English { get; set; }

        [Display(Name = "中文")]
        public bool Chinese { get; set; }

        [Display(Name = "हिंदी")]
        public bool Indian { get; set; }

        [Display(Name = "한국어")]
        public bool Japanese { get; set; }

        [Display(Name = "日本語")]
        public bool Korean { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(NullDisplayText = "0", DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Duration Day")]
        public int DurationDay { get; set; }

        [Required]
        [Range(0, 24)]
        [Display(Name = "Duration Hour")]
        public byte DurationHour { get; set; }

        //[NotMapped]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid Format. HH:MM (e.g. 07:45)")]
        [Display(Name = "Assembling Time")]
        public string AssemblingTimeString { get; set; }

        //[NotMapped]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid Format. HH:MM (e.g. 08:00)")]
        [Display(Name = "Departure Time")]
        public string DepartureTimeString { get; set; }

        //[NotMapped]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid Format. HH:MM (e.g. 20:00)")]
        [Display(Name = "Return Time")]
        public string ReturnTimeString { get; set; }

        public byte[] ImageData { get; set; }

        //[NotMapped]
        [Display(Name = "Image")]
        //[Required(ErrorMessage = "Please select a file")]
        public HttpPostedFileBase File { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        [Display(Name = "Redirect URL")]
        [StringLength(500)]
        [RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Invalid URL")]
        public string RedirectURL { get; set; }

        [StringLength(4000)]
        public string Term { get; set; }

        [Required]
        public virtual string icityid { get; set; }
        [Required]
        public virtual string istateid { get; set; }
        [Required]
        public virtual string icountryid { get; set; }

        public SelectList getCountry()
        {
            IEnumerable<SelectListItem> countryList = (from s in db.Country
                                                       select s).AsEnumerable().Select(s => new SelectListItem() { Text = s.CountryName, Value = s.CountryID.ToString() });
            return new SelectList(countryList, "Value", "Text", icountryid);
        }

        public SelectList getState()
        {
            IEnumerable<SelectListItem> stateList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(icityid))
            {
                int _countryId = Convert.ToInt32(icountryid);
                stateList = (from c in db.State
                             where c.CountryID == _countryId
                             select c).AsEnumerable().Select(c => new SelectListItem() { Text = c.StateName, Value = c.StateID.ToString() });
            }
            return new SelectList(stateList, "Value", "Text", istateid);
        }

        public SelectList getCity()
        {
            IEnumerable<SelectListItem> cityList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(icountryid) && !string.IsNullOrEmpty(istateid))
            {
                int _stateId = Convert.ToInt32(istateid);
                int _countryId = Convert.ToInt32(icountryid);
                cityList = (from c in db.City
                            join s in db.State on c.StateID equals s.StateID
                            where c.StateID == _stateId && s.CountryID == _countryId
                            select c).AsEnumerable().Select(c => new SelectListItem() { Text = c.CityName, Value = c.CityID.ToString() });
            }
            return new SelectList(cityList, "Value", "Text", icityid);
        }
    }

}