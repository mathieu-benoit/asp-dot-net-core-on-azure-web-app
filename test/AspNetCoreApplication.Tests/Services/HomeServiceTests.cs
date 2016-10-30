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
        public void TestAboutNotNullOrWhiteSpace() 
        {
            //Arrange

            //Act
            var aboutValue = _homeService.About;

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(aboutValue));
        }

        [Fact]
        public void TestContactNotNullOrWhiteSpace()
        {
            //Arrange

            //Act
            var contactValue = _homeService.Contact;

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(contactValue));
        }
    }
}
