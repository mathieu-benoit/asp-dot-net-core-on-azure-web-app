using AspNetCoreApplication.Services;
using Xunit;

namespace AspNetCoreApplication.Tests.Services
{
    public class HomeServiceTests
    {
        private IHomeService _homeService;
        public HomeServiceTests()
        {
            _homeService = new HomeService();
        }

        [Fact]
        public void About_NotNullNorWhiteSpace() 
        {
            //Arrange

            //Act
            var aboutValue = _homeService.About;

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(aboutValue));
        }

        [Fact]
        public void Contact_NotNullNorWhiteSpace()
        {
            //Arrange

            //Act
            var contactValue = _homeService.Contact;

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(contactValue));
        }
    }
}
