using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationUITest
{
    [TestClass]
    public class BingAutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        public BingAutomatedUITests()
        {
            _driver = new ChromeDriver(Directory.GetCurrentDirectory());
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [TestMethod]
        public void Index_WhenExecuted_ReturnsBingView()
        {
            _driver.Navigate().GoToUrl("https://www.bing.com/");

            Assert.AreEqual("Bing", _driver.Title);
        }

        [TestMethod]
        public void Bing_WhenClickImage_ReturnImageView()
        {
            //var CreateWait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(5));
            _driver.Navigate().GoToUrl("https://www.bing.com/");
            _driver.FindElement(By.LinkText("Images")).Click();

            Assert.AreEqual("Bing Image Trending", _driver.Title);
        }

        [TestMethod]
        public void Bing_WhenSearchTopic_ReturnsTopicPageView()
        {
            _driver.Navigate().GoToUrl("https://www.bing.com/");
            var input = _driver.FindElement(By.Id("sb_form_q"));
            input.SendKeys("seattle");
            //input.SendKeys(Keys.Enter);
            input.Submit();
           

            Assert.AreEqual("seattle - Bing", _driver.Title);
        }
    }
}
