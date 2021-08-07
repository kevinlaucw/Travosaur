using System.Data;
using System.Data.SqlClient;
using Travosaur.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Travosaur.Services
{
    public class TourService
    {
        private ApplicationDbContext db;

        public TourService(ApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public void UpdateAllTours()
        {
            using (db)
            {
                try
                {
                    db.Database.Connection.Open();
                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandText = "dbo.UpdateAllTours";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

        public bool IsDate(string input)
        {
            DateTime result;
            return DateTime.TryParse(input, out result);
        }

        public IEnumerable<TourViewModel> GetAllTours()
        {
            List<TourViewModel> tours = new List<TourViewModel>();
            IDataReader dataReader = null;

            using (db)
            {
                try
                {
                    db.Database.Connection.Open();
                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandText = "dbo.GetAllTours";
                    cmd.CommandType = CommandType.StoredProcedure;
                    dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        tours.Add(SetTourViewModelData(dataReader));
                    }

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            return tours;
        }

        public IEnumerable<TourViewModel> GetTour(string searchString,
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
            List<TourViewModel> tours = new List<TourViewModel>();
            IDataReader dataReader = null;

            using (db)
            {
                try
                {
                    db.Database.Connection.Open();
                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandText = "dbo.GetTour";
                    cmd.Parameters.Add(new SqlParameter("searchString", searchString));
                    cmd.Parameters.Add(new SqlParameter("fromDate", fromDate));
                    cmd.Parameters.Add(new SqlParameter("toDate", toDate));
                    cmd.Parameters.Add(new SqlParameter("english", english == null ? 0 : 1));
                    cmd.Parameters.Add(new SqlParameter("chinese", chinese == null ? 0 : 1));
                    cmd.Parameters.Add(new SqlParameter("indian", indian == null ? 0 : 1));
                    cmd.Parameters.Add(new SqlParameter("korean", korean == null ? 0 : 1));
                    cmd.Parameters.Add(new SqlParameter("japanese", japanese == null ? 0 : 1));
                    cmd.Parameters.Add(new SqlParameter("fromPrice", fromPrice));
                    cmd.Parameters.Add(new SqlParameter("toPrice", toPrice));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        tours.Add(SetTourViewModelData(dataReader));
                    }

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            return tours;
        }

        public TourViewModel SetTourViewModelData(IDataReader dataReader)
        {
            TourViewModel t = new TourViewModel();
            t.TourID = Convert.ToInt32(dataReader["TourID"]);
            t.TourName = dataReader["TourName"].ToString();
            t.Description = dataReader["Description"].ToString();
            t.ShortDescription = dataReader["ShortDescription"].ToString();
            t.TourCode = dataReader["TourCode"].ToString();
            t.TourTypeID = DBNull.Value.Equals(dataReader["TourTypeID"]) ? 0 : Convert.ToInt32(dataReader["TourTypeID"]);
            t.English = Convert.ToBoolean(dataReader["English"]);
            t.Chinese = Convert.ToBoolean(dataReader["Chinese"]);
            t.Indian = Convert.ToBoolean(dataReader["Indian"]);
            t.Korean = Convert.ToBoolean(dataReader["Korean"]);
            t.Japanese = Convert.ToBoolean(dataReader["Japanese"]);
            t.CityID = Convert.ToInt32(dataReader["CityID"]);
            t.CityName = dataReader["CityName"].ToString();
            t.StateName = dataReader["StateName"].ToString();
            t.CountryName = dataReader["CountryName"].ToString();
            t.Currency = dataReader["Currency"].ToString();
            t.Price = Convert.ToDecimal(dataReader["Price"]);
            t.RoundedPrice = Convert.ToDecimal(dataReader["Price"]);
            t.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
            t.EndDate = IsDate(dataReader["EndDate"].ToString()) ? t.EndDate = Convert.ToDateTime(dataReader["EndDate"]) : null;
            t.Active = Convert.ToBoolean(dataReader["Active"]);
            t.DateCreated = Convert.ToDateTime(dataReader["DateCreated"]);
            t.CreatedBy = dataReader["CreatedBy"].ToString();
            t.CreatedByName = dataReader["CreatedByName"].ToString();
            t.LastUpdated = IsDate(dataReader["LastUpdated"].ToString()) ? t.LastUpdated = Convert.ToDateTime(dataReader["LastUpdated"]) : null;
            t.LastUpdatedBy = dataReader["LastUpdatedBy"].ToString();
            t.DurationDay = Convert.ToInt32(dataReader["DurationDay"]);
            t.DurationHour = Convert.ToByte(dataReader["DurationHour"]);
            t.ImageData = (byte[])dataReader["ImageData"];
            t.RedirectURL = dataReader["RedirectURL"].ToString();
            t.Term = dataReader["Term"].ToString();
            return t;
        }

        public IEnumerable<TourViewModel> GetMyTrips(string userId)
        {  
            return (from t in db.TourView.AsEnumerable()
                         where (t.CreatedBy == userId)
                         orderby t.TourName
                         select new TourViewModel
                         {
                             TourID = t.TourID,
                             TourName = t.TourName,
                             ShortDescription = t.ShortDescription,
                             Description = t.Description,
                             TourCode = t.TourCode,
                             TourTypeID = t.TourTypeID,
                             English = t.English,
                             Chinese = t.Chinese,
                             Indian = t.Indian,
                             Korean = t.Korean,
                             Japanese = t.Japanese,
                             CityID = t.CityID,
                             CityName = t.CityName,
                             StateName = t.StateName,
                             CountryName = t.CountryName,
                             Currency = t.Currency,
                             Price = t.Price,
                             RoundedPrice = t.Price,
                             StartDate = t.StartDate,
                             EndDate = t.EndDate,
                             DateCreated = t.DateCreated,
                             CreatedBy = t.CreatedBy,
                             CreatedByName = t.CreatedByDisplayName,
                             LastUpdated = t.LastUpdated,
                             LastUpdatedBy = t.LastUpdatedBy,
                             DurationDay = t.DurationDay,
                             DurationHour = t.DurationHour,
                             ImageData = t.ImageData,
                             RedirectURL = t.RedirectURL,
                             Term = t.Term
                         });
        }

        public IEnumerable<TourViewModel> GetMyListings(string userId)
        {
            return (from t in db.TourView.AsEnumerable()
                         where (t.CreatedBy == userId)
                         orderby t.TourName
                         select new TourViewModel
                         {
                             TourID = t.TourID,
                             TourName = t.TourName,
                             ShortDescription = t.ShortDescription,
                             Description = t.Description,
                             TourCode = t.TourCode,
                             TourTypeID = t.TourTypeID,
                             English = t.English,
                             Chinese = t.Chinese,
                             Indian = t.Indian,
                             Korean = t.Korean,
                             Japanese = t.Japanese,
                             CityID = t.CityID,
                             CityName = t.CityName,
                             StateName = t.StateName,
                             CountryName = t.CountryName,
                             Currency = t.Currency,
                             Price = t.Price,
                             RoundedPrice = t.Price,
                             StartDate = t.StartDate,
                             EndDate = t.EndDate,
                             DateCreated = t.DateCreated,
                             CreatedBy = t.CreatedBy,
                             LastUpdated = t.LastUpdated,
                             LastUpdatedBy = t.LastUpdatedBy,
                             DurationDay = t.DurationDay,
                             DurationHour = t.DurationHour,
                             ImageData = t.ImageData,
                             RedirectURL = t.RedirectURL,
                             Term = t.Term
                         });
        }

        public IEnumerable<TourViewModel> GetUserTourByID(int? tourId, string userId)
        {
            List<TourViewModel> tour = new List<TourViewModel>();
            IDataReader dataReader = null;

            using (db)
            {
                try
                {
                    db.Database.Connection.Open();
                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandText = "dbo.GetUserTourByID";
                    cmd.Parameters.Add(new SqlParameter("tourId", tourId));
                    cmd.Parameters.Add(new SqlParameter("userId", userId));
                    cmd.CommandType = CommandType.StoredProcedure;
                    dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        tour.Add(SetTourViewModelData(dataReader));
                    }

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }

            return tour;
        }

        // Determines whether the collection is null or contains no elements.
        public bool IsEmptyIEnumerable<T>(IEnumerable<T> source)
        {
            return !source.Any();
        }

        //
        // ResizeImage: Resize image to smaller size before storing into database
        public byte[] ResizeImage(double scaleFactor, Stream sourcePath)
        {
            var image = System.Drawing.Image.FromStream(sourcePath);
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailBitmap = new Bitmap(newWidth, newHeight);
            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);

            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            MemoryStream stream = new MemoryStream();
            thumbnailBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] data = stream.ToArray();

            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();

            return data;
        }

    }
}