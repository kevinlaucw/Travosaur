using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Travosaur;
using Travosaur.Controllers;
using static Travosaur.Controllers.ManageController;
using Travosaur.Models;

namespace Travosaur.Tests.Controllers
{
    [TestClass]
    public class TourControllerTest
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [TestMethod]
        public void Index()
        {
            // Arrange
            string searchString = "great";
            DateTime? fromDate = null;
            DateTime? toDate = null;
            string english = "on";
            string chinese = null;
            int? fromPrice = 0;
            int? toPrice = 10000;

            // Act
            var tours = (from t in db.Tour.AsEnumerable()
                         join u in db.Users.AsEnumerable() on t.CreatedBy equals u.Id
                         join ct in db.City.AsEnumerable() on t.CityID equals ct.CityID
                         join s in db.State.AsEnumerable() on ct.StateID equals s.StateID
                         join c in db.Country.AsEnumerable() on s.CountryID equals c.CountryID
                         where (t.Active == true && t.StartDate <= DateTime.Today && (t.EndDate == null || t.EndDate > DateTime.Today)) &&
                                 (searchString == null || t.TourName.Contains(searchString) || t.ShortDescription.Contains(searchString)) &&
                                 (
                                    (fromDate == null && toDate == null) ||
                                    (fromDate <= t.StartDate && toDate >= DateTime.Today && (t.EndDate == null || (t.EndDate >= toDate)))
                                 ) &&
                                 (
                                    (
                                        english == null || (
                                                                (chinese == null && t.English == true) ||
                                                                (chinese != null && (t.Chinese == true || t.English == true))
                                                           )
                                    ) &&
                                    (
                                        chinese == null || (
                                                                (english == null && t.Chinese == true) ||
                                                                (english != null && (t.Chinese == true || t.English == true))
                                                           )
                                    )
                                 ) &&
                                 (
                                    fromPrice == null || (t.Price >= fromPrice && t.Price <= toPrice)
                                 )
                         orderby t.Price
                         select new TourViewModel
                         {
                             TourName = t.TourName,
                             ShortDescription = t.ShortDescription,
                             //Description = t.Description,
                             TourCode = t.TourCode,
                             TourTypeID = t.TourTypeID,
                             English = t.English,
                             Chinese = t.Chinese,
                             CityID = t.CityID,
                             CityName = ct.CityName,
                             StateName = s.StateName,
                             CountryName = c.CountryName,
                             Currency = t.Currency,
                             Price = t.Price,
                             RoundedPrice = t.Price,
                             StartDate = t.StartDate,
                             EndDate = t.EndDate,
                             Active = t.Active,
                             DateCreated = t.DateCreated,
                             CreatedBy = t.CreatedBy,
                             CreatedByName = u.FirstName,
                             LastUpdated = t.LastUpdated,
                             LastUpdatedBy = t.LastUpdatedBy,
                             DurationDay = t.DurationDay,
                             DurationHour = t.DurationHour,
                             ImageData = t.ImageData,
                             RedirectURL = t.RedirectURL,
                             Term = t.Term
                         });

            // Assert
            Assert.IsNotNull(tours);

        }

    }
}
