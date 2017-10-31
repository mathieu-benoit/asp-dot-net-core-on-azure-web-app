using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCoreApplication.UITests
{
    [TestClass]
    public class HomePageTests : SeleniumTests
    {
        [TestMethod]
        [TestCategory("UITests")]
        public void Load_Homepage()
        {
            //Arrange
            var homepageUrl = BaseUrl;

            //Act
            Driver.Navigate().GoToUrl(homepageUrl);

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(Driver.Title));
            Assert.IsTrue(Driver.Title.Equals("Home Page - DemoApp"));
            Assert.IsFalse(string.IsNullOrEmpty(Driver.Url));
            Assert.IsTrue(Driver.Url == homepageUrl);
        }
    }
}