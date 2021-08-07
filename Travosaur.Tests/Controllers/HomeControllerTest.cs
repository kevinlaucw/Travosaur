﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Travosaur;
using Travosaur.Controllers;

namespace Travosaur.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Copyright()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Copyright() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FAQ()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.FAQ() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Privacy()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Privacy() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Terms()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Terms() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
