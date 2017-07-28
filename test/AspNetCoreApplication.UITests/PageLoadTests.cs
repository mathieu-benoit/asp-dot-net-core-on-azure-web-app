using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace AspNetCoreApplication.UITests
{
    [TestClass]
    public class PageLoadTests
    {
        public TestContext TestContext { get; set; }
        private string baseUrl = "http://localhost:63360/";
        private RemoteWebDriver driver;
        private string browser = "PhantomJS";

        [TestInitialize()]
        public void TestInitialize()
        {
            //Manage BaseUrl from TestContext.
            if (TestContext.Properties["BaseUrl"] != null)
            {
                //Set the BaseURL from a build
                baseUrl = TestContext.Properties["BaseUrl"].ToString();
            }

            //Manage Browser from TestContext.
            browser = TestContext.Properties["Browser"] != null ? TestContext.Properties["Browser"].ToString() : "PhantomJS";
            switch (browser)
            {
                case "Firefox":
                    driver = new FirefoxDriver();
                    break;
                case "Chrome":
                    driver = new ChromeDriver();
                    break;
                case "IE":
                    driver = new InternetExplorerDriver();
                    break;
                case "PhantomJS":
                    driver = new PhantomJSDriver();
                    break;
                case "Edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    driver = new PhantomJSDriver();
                    break;
            }
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            driver.Quit();
        }

        [TestMethod]
        [TestCategory("UITests")]
        public void Load_Homepage()
        {
            //Arrange
            var homepageUrl = baseUrl;

            //Act
            driver.Navigate().GoToUrl(homepageUrl);

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(driver.Title));
            Assert.IsTrue(driver.Title.Equals("Home Page - DemoApp"));
            Assert.IsFalse(string.IsNullOrEmpty(driver.Url));
            Assert.IsTrue(driver.Url == homepageUrl);
        }

        [TestMethod]
        [TestCategory("UITests")]
        public void Load_Aboutpage()
        {
            //Arrange
            var aboutpageUrl = baseUrl + "home/about";

            //Act
            driver.Navigate().GoToUrl(aboutpageUrl);

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(driver.Title));
            Assert.IsTrue(driver.Title.Equals("About - DemoApp"));
            Assert.IsFalse(string.IsNullOrEmpty(driver.Url));
            Assert.IsTrue(driver.Url == aboutpageUrl);
        }
    }
}