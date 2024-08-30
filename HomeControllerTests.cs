using AuthUser.Controllers;
using AuthUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;


namespace AuthUser.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<ILogger<HomeController>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [TestMethod]
        public void Index_ReturnsAViewResult()
        {
            // Act
            var result = _controller.Index() as ViewResult;

         
            Assert.IsNotNull(result);
            // Adjust the expected view name as needed
            Assert.AreEqual("Index", result.ViewName);
        }

   
    }
}





