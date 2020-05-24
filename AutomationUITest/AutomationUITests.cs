using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationUITest
{
    [TestClass]
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        public AutomatedUITests()
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

        [TestMethod]
        public void MovieMvc_Index_WhenExecuted_ReturnsAResultView()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/");

            Assert.AreEqual("Home Page - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_Index_WhenNavigateToPrivacy_ReturnsAResultViewOfPrivacy()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/");
            _driver.FindElement(By.PartialLinkText("Privacy")).Click();

            Assert.AreEqual("Privacy Policy - Movie App", _driver.Title);

        }



    }
}
