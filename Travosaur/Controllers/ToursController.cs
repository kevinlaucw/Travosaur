using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Travosaur.Models;

namespace Travosaur.Controllers
{
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tours
        public ActionResult Index()
        {
            var tour = db.Tour.Include(t => t.City);
            return View(tour.ToList());
        }

        // GET: Tours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tour.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.City, "CityID", "CityName");
            return View();
        }

        // POST: Tours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourID,TourName,ShortDescription,Description,TourCode,TourTypeID,CityID,Currency,Price,StartDate,EndDate,DateCreated,CreatedBy,LastUpdated,LastUpdatedBy,DurationDay,DurationHour,AssemblingTime,DepartureTime,ReturnTime,ImageData,RedirectURL,Term,English,Chinese,Indian,Japanese,Korean,Active,Deleted")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tour.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.City, "CityID", "CityName", tour.CityID);
            return View(tour);
        }

        // GET: Tours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tour.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.City, "CityID", "CityName", tour.CityID);
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourID,TourName,ShortDescription,Description,TourCode,TourTypeID,CityID,Currency,Price,StartDate,EndDate,DateCreated,CreatedBy,LastUpdated,LastUpdatedBy,DurationDay,DurationHour,AssemblingTime,DepartureTime,ReturnTime,ImageData,RedirectURL,Term,English,Chinese,Indian,Japanese,Korean,Active,Deleted")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.City, "CityID", "CityName", tour.CityID);
            return View(tour);
        }

        // GET: Tours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tour.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTour(int id)
        {
            Tour tour = db.Tour.Find(id);
            db.Tour.Remove(tour);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
