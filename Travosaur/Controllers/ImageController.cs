using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using Travosaur.Models;

namespace Travosaur.Controllers
{
    public class ImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Image
        public ActionResult Index()
        {
            IQueryable<Image> images = db.Image;
            var sql = images.ToString();
            return View(images.ToList());
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Image I)
        {
            // Apply Validation Here

            if (I.File.ContentLength > (2 * 1024 * 1024))
            {
                ModelState.AddModelError("CustomError", "File size must be less than 2 MB");
                return View();
            }
            if (!(I.File.ContentType == "image/jpeg" || I.File.ContentType == "image/gif" || I.File.ContentType == "image/png"))
            {
                ModelState.AddModelError("CustomError", "File type allowed : jpeg, gif and png");
                return View();
            }

            I.ImageName = I.File.FileName;
            I.ImageSize = I.File.ContentLength;

            byte[] data = new byte[I.File.ContentLength];
            I.File.InputStream.Read(data, 0, I.File.ContentLength);

            I.ImageData = data;

            try
            {
                if (ModelState.IsValid)
                {
                    db.Image.Add(I);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }

            return RedirectToAction("Index");

        }

    }
}