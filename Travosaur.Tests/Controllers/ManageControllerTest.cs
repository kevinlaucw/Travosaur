using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Travosaur;
using Travosaur.Controllers;
using static Travosaur.Controllers.ManageController;

namespace Travosaur.Tests.Controllers
{
    [TestClass]
    public class ManageControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddPhoneNumber()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            ViewResult result = controller.AddPhoneNumber() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ChangePassword()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            ViewResult result = controller.ChangePassword() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SetPassword()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            ViewResult result = controller.SetPassword() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Inbox()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            ViewResult result = controller.Inbox() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MyListings()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            //ViewResult result = controller.MyListings() as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MyTrips()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            //ViewResult result = controller.MyTrips() as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddTour()
        {
            // Arrange
            ManageController controller = new ManageController();

            // Act
            ViewResult result = controller.AddTour() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
