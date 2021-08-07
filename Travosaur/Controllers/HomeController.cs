using System;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using Travosaur.Models;
using System.Reflection;

namespace Travosaur.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Search")]
        public ActionResult Index(string searchString)
        {
            return this.RedirectToAction("Index", "Tour", new { searchString = searchString });
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Subscribe")]
        public ActionResult Index(Subscriber model)
        {
            bool subscriberEmailExists = db.Subscriber.Any(sub => sub.Email.Equals(model.Email));
            if (subscriberEmailExists)
            {
                ViewBag.Subscribe = "You have previously subscribed with Travosaur.";
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Subscriber.Add(model);
                    db.SaveChanges();
                    ViewBag.Subscribe = "Thanks for subscribing with Travosaur!";
                    return View(model);
                }           
                return View(model);
            }         
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.SendMessage = "Have a question? Send us a message.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactFormModel model)
        {
            ViewBag.SendMessage = "Thanks, we got your message!";
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("support@travosaur.com"));
                message.Subject = "Travosaur - Contact Us";
                message.Body = string.Format(body, model.Name, model.Email, model.Message);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return PartialView("_ContactThanks");
                }
            }
            return View(model);
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Copyright()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult TourOperator()
        {
            return View();
        }
    }

    // Supporting multiple submit buttons on an ASP.NET MVC view. 
    // See http://blog.maartenballiauw.be/post/2009/11/26/Supporting-multiple-submit-buttons-on-an-ASPNET-MVC-view.aspx
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string MatchFormKey { get; set; }
        public string MatchFormValue { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
        }
    }
}