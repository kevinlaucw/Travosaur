using System;
using System.Web.Mvc;
using PagedList;
using Travosaur.Models;
using Travosaur.Services;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;

namespace Travosaur.Controllers
{
    public class TourController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();     

        // GET: Tour
        public ActionResult Index(string searchString,
                                    DateTime? fromDate,
                                    DateTime? toDate, 
                                    string english, 
                                    string chinese,
                                    string indian,
                                    string korean,
                                    string japanese,
                                    int? fromPrice,
                                    int? toPrice, 
                                    int? page)
        {
            //construct FromPrices dropdownlist
            ViewBag.FromPriceList = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "$0", Value = "0", Selected = true },
                    new SelectListItem { Text = "$50", Value = "50" },
                    new SelectListItem { Text = "$100", Value = "100" },
                    new SelectListItem { Text = "$200", Value = "200" },
                    new SelectListItem { Text = "$300", Value = "300" },
                    new SelectListItem { Text = "$400", Value = "400" },
                    new SelectListItem { Text = "$500", Value = "500" },
                    new SelectListItem { Text = "$800", Value = "800" },
                    new SelectListItem { Text = "$1000", Value = "1000" },
                    new SelectListItem { Text = "$2000", Value = "2000" },
                    new SelectListItem { Text = "$3000", Value = "3000" },
                    new SelectListItem { Text = "$4000", Value = "4000" },
                    new SelectListItem { Text = "$5000", Value = "5000" },
                    new SelectListItem { Text = "$10000", Value = "10000" },
                }, "Value", "Text", 1
            );

            //construct ToPrices dropdownlist
            ViewBag.ToPriceList = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "$10000", Value = "10000", Selected = true },
                    new SelectListItem { Text = "$50", Value = "50" },
                    new SelectListItem { Text = "$100", Value = "100" },
                    new SelectListItem { Text = "$200", Value = "200" },
                    new SelectListItem { Text = "$300", Value = "300" },
                    new SelectListItem { Text = "$400", Value = "400" },
                    new SelectListItem { Text = "$500", Value = "500" },
                    new SelectListItem { Text = "$800", Value = "800" },
                    new SelectListItem { Text = "$1000", Value = "1000" },
                    new SelectListItem { Text = "$2000", Value = "2000" },
                    new SelectListItem { Text = "$3000", Value = "3000" },
                    new SelectListItem { Text = "$4000", Value = "4000" },
                    new SelectListItem { Text = "$5000", Value = "5000" },
                }, "Value", "Text", 1
            );

            searchString = searchString == null ? "" : searchString.Trim();

            //default to english and chinese tours or when no languages were selected
            if (english == null && chinese == null && indian == null && korean == null && japanese == null)
            {
                english = "on";
                chinese = "on";
            }

            ViewBag.SearchString = searchString;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            //store values of language checkboxes
            ViewBag.English = english == null ? "" : english == "on" ? "on" : "";
            ViewBag.Chinese = chinese == null ? "" : chinese == "on" ? "on" : "";
            ViewBag.Indian = indian == null ? "" : indian == "on" ? "on" : "";
            ViewBag.Korean = korean == null ? "" : korean == "on" ? "on" : "";
            ViewBag.Japanese = japanese == null ? "" : japanese == "on" ? "on" : "";
            ViewBag.FromPrice = fromPrice ?? 0;
            ViewBag.ToPrice = toPrice ?? 10000;

            //set language checkboxes based on query strings' values
            ViewBag.CheckedEnglish = english == null ? "" : "checked";
            ViewBag.CheckedChinese = chinese == null ? "" : "checked";
            ViewBag.CheckedIndian = indian == null ? "" : "checked";
            ViewBag.CheckedKorean = korean == null ? "" : "checked";
            ViewBag.CheckedJapanese = japanese == null ? "" : "checked";

            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            var tours = service.GetTour(searchString, fromDate, toDate, english, chinese, indian, korean, japanese, fromPrice, toPrice);

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(tours.ToPagedList(pageNumber, pageSize));
        }

        // POST: Tour
        [HttpPost]
        public ActionResult Index(string searchString,
                                    DateTime? fromDate,
                                    DateTime? toDate, 
                                    string english, 
                                    string chinese,
                                    string indian,
                                    string korean,
                                    string japanese,
                                    int? fromPrice,
                                    int? toPrice)
        {
            return RedirectToAction("Index", "Tour", new { searchString = searchString,
                                                            fromDate = fromDate,
                                                            toDate = toDate,
                                                            english = english,
                                                            chinese = chinese,
                                                            indian = indian,
                                                            korean = korean,
                                                            japanese = japanese,
                                                            fromPrice = fromPrice,
                                                            toPrice = toPrice });
        }

    }
}