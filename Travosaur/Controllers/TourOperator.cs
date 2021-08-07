using System.Web.Mvc;
using PagedList;
using Travosaur.Models;
using Travosaur.Services;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace Travosaur.Controllers
{
    public class TourOperatorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();     

        public ActionResult Index(string userId, int? page)
        {
            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            var tours = service.GetMyListings(userId);
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(tours.ToPagedList(pageNumber, pageSize));
        }

    }
}