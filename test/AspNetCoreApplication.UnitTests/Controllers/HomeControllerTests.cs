using AspNetCoreApplication.Controllers;
using AspNetCoreApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AspNetCoreApplication.UnitTests.Services
{
    public class HomeControllerTests
    {
        [Fact]
        public void About_NotNullAndMessageViewDataFillOut() 
        {
            //Arrange
            var homeService = new HomeService();//TODO: Use Moq framework instead.
            var homeController = new HomeController(homeService);

            //Act
            var aboutResult = homeController.About();

            //Assert
            Assert.IsType<ViewResult>(aboutResult);
            Assert.True(homeController.ViewData.Values.Contains(homeService.About));
        }

        [Fact]
        public void Contact_NotNullAndMessageViewDataFillOut()
        {
            //Arrange
            var homeService = new HomeService();//TODO: Use Moq framework instead.
            var homeController = new HomeController(homeService);

            //Act
            var contactResult = homeController.Contact();

            //Assert
            Assert.IsType<ViewResult>(contactResult);
            Assert.True(homeController.ViewData.Values.Contains(homeService.Contact));
        }
    }
}
