using AspNetCoreApplication.Services;
using Xunit;

namespace AspNetCoreApplication.UnitTests.Services
{
    public class HomeServiceTests
    {
        [Fact]
        public void About_NotNullNorWhiteSpace() 
        {
            //Arrange
            var homeService = new HomeService();//TODO: Use Moq framework instead.

            //Act
            var aboutValue = homeService.About;

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(aboutValue));
        }

        [Fact]
        public void Contact_NotNullNorWhiteSpace()
        {
            //Arrange
            var homeService = new HomeService();//TODO: Use Moq framework instead.

            //Act
            var contactValue = homeService.Contact;

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(contactValue));
        }
    }
}
