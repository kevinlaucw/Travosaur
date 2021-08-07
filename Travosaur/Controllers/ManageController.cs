using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Travosaur.Models;
using Travosaur.Services;

namespace Travosaur.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        //
        // GET: /Manage/Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Manage/Settings
        public async Task<ActionResult> Settings(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.ChangeDetailsSuccess ? "Your details have been updated"
                : "";

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var model = new SettingsViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(currentUser.Id),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(currentUser.Id),
                Logins = await UserManager.GetLoginsAsync(currentUser.Id),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(currentUser.Id),
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                DisplayName = currentUser.DisplayName,
                Subscribed = currentUser.Subscribed,
                Email = currentUser.Email
            };
            return View(model);
        }

        //
        // POST: /Manage/Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Settings(SettingsViewModel model, string firstname, string lastname, string displayname, bool subscribed)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var OldFirstName = user.FirstName;
            var OldLastName = user.LastName;
            var OldDisplayName = user.DisplayName;
            var OldSubscribed = user.Subscribed;

            if ((firstname != OldFirstName) || (lastname != OldLastName) || (displayname != OldDisplayName) || (subscribed != OldSubscribed))
            {
                user.FirstName = firstname;
                user.LastName = lastname;
                user.DisplayName = displayname;
                user.Subscribed = subscribed;
 
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    if (OldDisplayName == null && string.IsNullOrWhiteSpace(displayname) == false)
                    {
                        UserManager.AddClaim(user.Id, new Claim(ClaimTypes.GivenName, model.DisplayName));
                    }
                    else if (displayname != OldDisplayName)
                    { 
                        UserManager.RemoveClaim(user.Id, new Claim(ClaimTypes.GivenName, OldDisplayName));
                        UserManager.AddClaim(user.Id, new Claim(ClaimTypes.GivenName, model.DisplayName));
                    }
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Settings", new { Message = ManageMessageId.ChangeDetailsSuccess });
                }
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction("Settings");
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Settings", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Settings", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Settings", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Settings", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Settings", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Settings", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Settings", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        //
        // GET: /Manage/Inbox
        public ActionResult Inbox()
        {
            return View();
        }

        //
        // GET: /Manage/MyListings
        public ActionResult MyListings(int? page)
        {       
            if (HasPhoneNumber() == true)
            {
                ViewBag.HasPhoneNumber = "true";
            }
            else
            {
                ViewBag.HasPhoneNumber = "false";
            }

            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            var tours = service.GetMyListings(User.Identity.GetUserId());

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(tours.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Manage/MyTrips
        public ActionResult MyTrips(int? page)
        {
            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            var tours = service.GetMyTrips(User.Identity.GetUserId());

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(tours.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Manage/ViewTour
        public ActionResult ViewTour(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (HasPhoneNumber() == true)
            {
                ViewBag.HasPhoneNumber = "true";
            }
            else
            {
                ViewBag.HasPhoneNumber = "false";
                return View();
            }

            var userId = User.Identity.GetUserId();
            var tour = db.Tour.Where(t => t.TourID == id && t.CreatedBy == userId).FirstOrDefault();

            if (tour == null)
            {
                return HttpNotFound();
            }

            int countryid = db.City.FirstOrDefault(c => c.CityID == tour.CityID).CountryID;
            int stateid = db.City.FirstOrDefault(c => c.CityID == tour.CityID).StateID;
            int cityid = tour.CityID;

            ViewBag.CountryName = db.Country.FirstOrDefault(c => c.CountryID == countryid).CountryName.ToString();
            ViewBag.StateName = db.State.FirstOrDefault(c => c.StateID == stateid).StateName.ToString();
            ViewBag.CityName = db.City.FirstOrDefault(c => c.CityID == cityid).CityName.ToString();

            return View(tour);
        }
       
        //
        // GET: /Manage/EditTour/5
        public ActionResult EditTour(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (HasPhoneNumber() == true)
            {
                ViewBag.HasPhoneNumber = "true";
            }
            else
            {
                ViewBag.HasPhoneNumber = "false";
                return View();
            }
            
            var model = new ManageTourViewModel
            {
                Currencies = db.Currency.Where(c => c.Active == true).ToList(),
            };

            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            var tour = service.GetUserTourByID(id, User.Identity.GetUserId());

            if (tour == null)
            {
                return HttpNotFound();
            }

            AssignTourData_GET(model, tour);

            string countryid = db.City.FirstOrDefault(c => c.CityID == model.CityID).CountryID.ToString();
            string stateid = db.City.FirstOrDefault(c => c.CityID == model.CityID).StateID.ToString();
            string cityid = model.CityID.ToString();

            //binding values to dropdownlists
            ViewBag.CountryList = getCountry(countryid);
            ViewBag.StateList = getState(countryid, stateid);
            ViewBag.CityList = getCity(countryid, stateid, cityid);
            ViewBag.CurrencyList = getCurrency(model.Currency); 

            return View(model);
        }

        //
        // POST: /Manage/EditTour/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTour(ManageTourViewModel t)
        //public ActionResult EditTour([Bind(Include = "TourID,TourName,ShortDescription,Description,TourCode,TourTypeID,CityID,Currency,Price,StartDate,EndDate,DateCreated,CreatedBy,LastUpdated,LastUpdatedBy,DurationDay,DurationHour,AssemblingTime,DepartureTime,ReturnTime,ImageData,RedirectURL,Term,English,Chinese,Indian,Japanese,Korean,Active,Deleted")] ManageTourViewModel t)
        {
            try
            {
                //binding selected values to dropdownlists after postback
                ViewBag.CountryList = getCountry(t.icountryid);
                ViewBag.StateList = getState(t.icityid, t.istateid);
                ViewBag.CityList = getCity(t.icountryid, t.istateid, t.icityid);
                ViewBag.CurrencyList = getCurrency(t.Currency);

                if (ModelState.IsValid)
                {
                    if (t.English == false && t.Chinese == false && t.Indian == false && t.Korean == false && t.Japanese == false)
                    {
                        ModelState.AddModelError("CustomLanguageError", "Please select at least one language");
                        return View();
                    }

                    //update image
                    if (!(t.File == null))
                    {
                        if (!(t.File.ContentType == "image/jpeg" || t.File.ContentType == "image/gif" || t.File.ContentType == "image/png"))
                        {
                            ModelState.AddModelError("CustomImageError", "File type allowed : jpeg, gif and png");
                            return View();
                        }

                        if (t.File.ContentLength > (8 * 1024 * 1024))
                        {
                            ModelState.AddModelError("CustomImageError", "File size must be equal or less than 8 MB");
                            return View();
                        }

                        //reduce image size if file size exceeds 2 MB
                        if (t.File.ContentLength > (2 * 1024 * 1024))
                        {
                            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
                            t.ImageData = service.ResizeImage(0.5, t.File.InputStream);
                        }
                        else
                        {
                            byte[] data = new byte[t.File.ContentLength];
                            t.File.InputStream.Read(data, 0, t.File.ContentLength);
                            t.ImageData = data;
                        }
                    }

                    //update tour
                    Tour tour = db.Tour.Find(t.TourID);

                    if (tour == null)
                    {
                        return HttpNotFound();
                    }

                    AssignTourData_POST(tour, t);

                    tour.LastUpdated = DateTime.Now;
                    tour.LastUpdatedBy = User.Identity.GetUserId();
                    
                    db.SaveChanges();

                    return RedirectToAction("ViewTour", new { id = t.TourID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }

            return RedirectToAction("EditTour");
        }

        //
        // GET: /Manage/DeleteTour/5
        public ActionResult DeleteTour(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (HasPhoneNumber() == true)
            {
                ViewBag.HasPhoneNumber = "true";
            }
            else
            {
                ViewBag.HasPhoneNumber = "false";
                return View();
            }

            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            var tour = service.GetUserTourByID(id, User.Identity.GetUserId());

            if (tour == null)
            {
                return HttpNotFound();
            }

            var model = new ManageTourViewModel();
            model.TourID = tour.FirstOrDefault().TourID;
            model.TourName = tour.FirstOrDefault().TourName;

            return View(model);
        }

        //
        // POST: /Manage/DeleteTour/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTour(int id)
        {
            Tour tour = db.Tour.Find(id);
            tour.Active = false;
            tour.Deleted = true;
            tour.LastUpdated = DateTime.Now;
            tour.LastUpdatedBy = User.Identity.GetUserId();
            db.SaveChanges();
            return RedirectToAction("MyListings");
        }

        //
        // GET: /Manage/AddTour
        public ActionResult AddTour()
        {
            if (HasPhoneNumber() == true)
            {
                ViewBag.HasPhoneNumber = "true";
            }
            else
            {
                ViewBag.HasPhoneNumber = "false";
                return View();
            }

            //populate dropdownlists
            ViewBag.CountryList = getCountry();
            ViewBag.StateList = new List<SelectListItem> { };  //blank dropdownlist
            ViewBag.CityList = new List<SelectListItem> { };  //blank no item
            ViewBag.CurrencyList = getCurrency();

            //set default values
            var model = new ManageTourViewModel
            {
                Currencies = db.Currency.Where(c => c.Active == true).ToList(),
                English = true,
                DurationDay = 0,
                DurationHour = 0,
                Active = true
            };

            return View(model);
        }

        //
        // POST: /Manage/AddTour
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTour(ManageTourViewModel t)
        //public ActionResult AddTour([Bind(Include = "TourID,TourName,ShortDescription,Description,TourCode,TourTypeID,CityID,Currency,Price,StartDate,EndDate,DateCreated,CreatedBy,LastUpdated,LastUpdatedBy,DurationDay,DurationHour,AssemblingTime,DepartureTime,ReturnTime,ImageData,RedirectURL,Term,English,Chinese,Indian,Japanese,Korean,Active,Deleted")] ManageTourViewModel t)
        {
            try
            {
                //binding selected values to dropdownlists after postback
                ViewBag.CountryList = getCountry(t.icountryid);
                ViewBag.StateList = getState(t.icityid, t.istateid);
                ViewBag.CityList = getCity(t.icountryid, t.istateid, t.icityid);
                ViewBag.CurrencyList = getCurrency(t.Currency);

                if (ModelState.IsValid)
                {
                    if (t.English == false && t.Chinese == false && t.Indian == false && t.Korean == false && t.Japanese == false)
                    {
                        ModelState.AddModelError("CustomLanguageError", "Please select at least one language");
                        return View();
                    }

                    //adding image
                    if (t.File == null)
                    {
                        ModelState.AddModelError("CustomImageError", "Please choose an image file");
                        return View();
                    }
                    else
                    {
                        if (!(t.File.ContentType == "image/jpeg" || t.File.ContentType == "image/gif" || t.File.ContentType == "image/png"))
                        {
                            ModelState.AddModelError("CustomImageError", "File type allowed : jpeg, gif and png");
                            return View();
                        }

                        if (t.File.ContentLength > (8 * 1024 * 1024))
                        {
                            ModelState.AddModelError("CustomImageError", "File size must be equal or less than 8 MB");
                            return View();
                        }

                        //reduce image size if file size exceeds 2 MB
                        if (t.File.ContentLength > (2 * 1024 * 1024))
                        {
                            var service = new TourService(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
                            t.ImageData = service.ResizeImage(0.5, t.File.InputStream);
                        }
                        else
                        {
                            byte[] data = new byte[t.File.ContentLength];
                            t.File.InputStream.Read(data, 0, t.File.ContentLength);
                            t.ImageData = data;
                        }
                    }

                    //adding a new tour
                    Tour tour = new Tour();

                    AssignTourData_POST(tour, t);

                    tour.DateCreated = DateTime.Now;
                    tour.CreatedBy = User.Identity.GetUserId();
    
                    db.Tour.Add(tour);
                    db.SaveChanges();

                    return RedirectToAction("MyListings");
                } 
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
            }

            return RedirectToAction("AddTour");
        }

        //Assign Tour Data (GET)
        public void AssignTourData_GET(ManageTourViewModel model, IEnumerable<TourViewModel> tour)
        {
            model.TourID = tour.FirstOrDefault().TourID;
            model.TourName = tour.FirstOrDefault().TourName;
            model.ShortDescription = tour.FirstOrDefault().ShortDescription;
            model.Description = WebUtility.HtmlDecode(tour.FirstOrDefault().Description);
            model.TourCode = tour.FirstOrDefault().TourCode;
            model.TourTypeID = tour.FirstOrDefault().TourTypeID;
            model.English = tour.FirstOrDefault().English;
            model.Chinese = tour.FirstOrDefault().Chinese;
            model.Indian = tour.FirstOrDefault().Indian;
            model.Korean = tour.FirstOrDefault().Korean;
            model.Japanese = tour.FirstOrDefault().Japanese;
            model.CityID = tour.FirstOrDefault().CityID;
            model.Currency = tour.FirstOrDefault().Currency;
            model.Price = tour.FirstOrDefault().Price;
            model.StartDate = tour.FirstOrDefault().StartDate;
            model.EndDate = tour.FirstOrDefault().EndDate;
            model.DateCreated = tour.FirstOrDefault().DateCreated;
            model.CreatedBy = tour.FirstOrDefault().CreatedBy;
            model.LastUpdated = tour.FirstOrDefault().LastUpdated;
            model.LastUpdatedBy = tour.FirstOrDefault().LastUpdatedBy;
            model.DurationDay = tour.FirstOrDefault().DurationDay;
            model.DurationHour = tour.FirstOrDefault().DurationHour;
            //model.AssemblingTime = tour.FirstOrDefault().AssemblingTime;
            //model.DepartureTime = tour.FirstOrDefault().DepartureTime;
            //model.ReturnTime = tour.FirstOrDefault().ReturnTime;
            //model.AssemblingTimeString = tour.FirstOrDefault().AssemblingTime?.ToString().Substring(0, 5);
            //model.DepartureTimeString = tour.FirstOrDefault().DepartureTime?.ToString().Substring(0, 5);
            //model.ReturnTimeString = tour.FirstOrDefault().ReturnTime?.ToString().Substring(0, 5);
            model.ImageData = tour.FirstOrDefault().ImageData;
            model.RedirectURL = tour.FirstOrDefault().RedirectURL;
            model.Term = tour.FirstOrDefault().Term;
            model.Active = tour.FirstOrDefault().Active;
        }

        //Assign Tour Data (POST)
        public void AssignTourData_POST(Tour tour, ManageTourViewModel t)
        {
            tour.TourName = t.TourName;
            tour.ShortDescription = t.ShortDescription;
            tour.Description = WebUtility.HtmlEncode(t.Description);
            tour.TourCode = t.TourCode;
            tour.TourTypeID = t.TourTypeID;
            tour.English = t.English;
            tour.Chinese = t.Chinese;
            tour.Indian = t.Indian;
            tour.Japanese = t.Japanese;
            tour.Korean = t.Korean;
            tour.CityID = Convert.ToInt32(t.icityid);
            tour.Currency = t.Currency;
            tour.Price = t.Price;
            tour.StartDate = t.StartDate;
            tour.EndDate = t.EndDate;
            tour.DurationDay = t.DurationDay;
            tour.DurationHour = t.DurationHour;
            //tour.AssemblingTime = (t.AssemblingTimeString == null) ? TimeSpan.Parse("0:00") : TimeSpan.Parse(t.AssemblingTimeString);
            //tour.DepartureTime = (t.DepartureTimeString == null) ? TimeSpan.Parse("0:00") : TimeSpan.Parse(t.DepartureTimeString);
            //tour.ReturnTime = (t.ReturnTimeString == null) ? TimeSpan.Parse("0:00") : TimeSpan.Parse(t.ReturnTimeString);

            if (!(t.ImageData == null))
            {
                tour.ImageData = t.ImageData;
            }

            tour.RedirectURL = t.RedirectURL;
            tour.Term = t.Term;
            tour.Active = t.Active;
        }

        public SelectList getCurrency(string selectCurrencyCode = null)
        {
            IEnumerable<SelectListItem> currencyList = (from c in db.Currency
                                                       where c.Active == true
                                                       select c).AsEnumerable().Select(c => new SelectListItem() { Text = c.CurrencyCode, Value = c.CurrencyCode.ToString() });
            return new SelectList(currencyList, "Value", "Text", selectCurrencyCode);
        }

        [HttpPost]
        public JsonResult getCountryJson(string selectCountryId = null)
        {
            return Json(getCountry(selectCountryId));
        }
        public SelectList getCountry(string selectCountryId = null)
        {
            IEnumerable<SelectListItem> countryList = (from c in db.Country
                                                       where c.Active == true
                                                       select c).AsEnumerable().Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryID.ToString() });
            return new SelectList(countryList, "Value", "Text", selectCountryId);
        }

        [HttpPost]
        public JsonResult getStateJson(string countryId, string selectStateId = null)
        {
            return Json(getState(countryId, selectStateId));
        }
        public SelectList getState(string countryId, string selectStateId = null)
        {
            IEnumerable<SelectListItem> stateList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(countryId))
            {
                int _countryId = Convert.ToInt32(countryId);
                stateList = (from m in db.State
                             where m.Active == true && m.CountryID == _countryId
                             select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.StateName, Value = m.StateID.ToString() });
            }
            return new SelectList(stateList, "Value", "Text", selectStateId);
        }

        [HttpPost]
        public JsonResult getCityJson(string countryId, string stateId, string selectCityId = null)
        {
            return Json(getCity(countryId, stateId, selectCityId));
        }
        public SelectList getCity(string countryId, string stateId, string selectCityId = null)
        {
            IEnumerable<SelectListItem> cityList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(countryId) && !string.IsNullOrEmpty(stateId))
            {
                int _stateId = Convert.ToInt32(stateId);
                int _countryId = Convert.ToInt32(countryId);
                cityList = (from m in db.City
                            join p in db.State on m.StateID equals p.StateID
                            where m.Active == true && m.StateID == _stateId && p.Active == true && p.CountryID == _countryId
                            select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.CityName, Value = m.CityID.ToString() });
            }
            return new SelectList(cityList, "Value", "Text", selectCityId);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            ChangeDetailsSuccess,
            Error
        }

#endregion
    }
}