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
            Assert.AreEqual("Home Page - DemoApp", Driver.Title);
            Assert.AreEqual(homepageUrl, Driver.Url);
        }
    }
}