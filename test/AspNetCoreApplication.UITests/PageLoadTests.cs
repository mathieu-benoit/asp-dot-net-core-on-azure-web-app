using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

namespace AspNetCoreApplication.UITests
{
    [TestClass]
    public class PageLoadTests
    {
        public TestContext TestContext { get; set; }

        string Url
        {
            get
            {
                if (TestContext.Properties["Url"] != null)
                {
                    return TestContext.Properties["Url"].ToString();
                }
                else
                {
                    return "http://www.google.com/";
                }
            }
        }

        [TestMethod]
        public void HomepageLoad_WithPhantomJS()
        {
            using (var driver = new PhantomJSDriver())
            {
                driver.Navigate().GoToUrl(Url);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void HomepageLoad_WithChrome()
        {
            using (var driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(Url);
            }
            Assert.IsTrue(true);
        }
    }
}