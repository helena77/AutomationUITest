using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

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

        [TestMethod]
        public void MovieMvc_Index_WhenNavigateToMovieApp_ReturnsAResultViewOfMovies()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/");
            _driver.FindElement(By.ClassName("navbar-brand")).Click();

            Assert.AreEqual("Index - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenExecuted_ReturnsAMovieList()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var list = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            Assert.AreEqual(5, list.Count);

        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenClickCreate_ReturnsACreateView()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            _driver.FindElement(By.PartialLinkText("Create")).Click();

            Assert.AreEqual("Create - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenClickEdit_ReturnsAEditView()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            _driver.FindElement(By.PartialLinkText("Edit")).Click();

            Assert.AreEqual("Edit - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenClickDetail_ReturnsAMovieView()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            _driver.FindElement(By.PartialLinkText("Detail")).Click();

            Assert.AreEqual("Details - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenClickDelete_ReturnsADeleteView()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            _driver.FindElement(By.PartialLinkText("Delete")).Click();

            Assert.AreEqual("Delete - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenCreated_ReturnsAIndexViewWithNewMovie()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            _driver.Navigate().GoToUrl("https://localhost:5001/Movies/Create");
            _driver.FindElement(By.Id("Title")).SendKeys("New Movie");
            _driver.FindElement(By.Id("ReleaseDate")).SendKeys("01/01/2020");
            _driver.FindElement(By.Id("Genre")).SendKeys("novel");
            _driver.FindElement(By.Id("Price")).SendKeys("5.99");
            _driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            
            var movieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            Assert.AreEqual("Index - Movie App", _driver.Title);
            Assert.AreEqual(oriMovieList.Count + 1, movieList.Count);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenCreateCanceled_ReturnsASameIndextViewAsBefore()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            _driver.Navigate().GoToUrl("https://localhost:5001/Movies/Create");
            _driver.FindElement(By.LinkText("Back to List")).Click();

            var movieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            Assert.AreEqual("Index - Movie App", _driver.Title);
            Assert.AreEqual(oriMovieList.Count, movieList.Count);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenEdited_ReturnsAIndexViewWithUpdatedMovie()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");           
            _driver.FindElement(By.LinkText("Edit")).Click();

            _driver.FindElement(By.Id("Title")).SendKeys(" II");
            _driver.FindElement(By.XPath("//input[@value='Save']")).Click();

            var updatedMovie = _driver.FindElement(By.CssSelector("[class='table'] tbody tr")).Text;

            Assert.IsTrue(updatedMovie.Contains("II"));
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenEditCanceled_ReturnsASameIndextViewAsBefore()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovie = _driver.FindElement(By.CssSelector("[class='table'] tbody tr")).Text;

            _driver.FindElement(By.LinkText("Edit")).Click();
            _driver.FindElement(By.LinkText("Back to List")).Click();

            var movie = _driver.FindElement(By.CssSelector("[class='table'] tbody tr")).Text;

            Assert.AreEqual("Index - Movie App", _driver.Title);
            Assert.AreEqual(oriMovie, movie);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenDetailExecuted_ReturnsAVieOfMovieDetail()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovie = _driver.FindElement(By.CssSelector("[class='table'] tbody tr td")).Text;
            _driver.FindElement(By.LinkText("Details")).Click();

            var element = _driver.FindElement(By.CssSelector("[class='row']")).Text;

            Assert.IsTrue(element.Contains(oriMovie));
        }

        [TestMethod]
         public void MovieMvc_MovieApp_WhenEditedInDetailPage_ReturnsAViewOfEdit()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            _driver.FindElement(By.LinkText("Details")).Click();
            _driver.FindElement(By.LinkText("Edit")).Click();

            Assert.AreEqual("Edit - Movie App", _driver.Title);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenDetailCanceled_ReturnsASameIndextViewAsBefore()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovie = _driver.FindElement(By.CssSelector("[class='table'] tbody tr")).Text;
            _driver.FindElement(By.LinkText("Details")).Click();

            _driver.FindElement(By.LinkText("Back to List")).Click();

            var movie = _driver.FindElement(By.CssSelector("[class='table'] tbody tr")).Text;

            Assert.AreEqual("Index - Movie App", _driver.Title);
            Assert.AreEqual(oriMovie, movie);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenDeleted_ReturnsAIndexViewWithRestMovie()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));
            _driver.FindElement(By.XPath("//table[@class='table']/tbody/tr[last()]/td/a[last()]")).Click();

            _driver.FindElement(By.XPath("//input[@value='Delete']")).Click();

            var movieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            Assert.AreEqual("Index - Movie App", _driver.Title);
            Assert.AreEqual(oriMovieList.Count - 1, movieList.Count);
        }

        [TestMethod]
        public void MovieMvc_MovieApp_WhenDeleteCancled_ReturnsASameIndextViewAsBefore()
        {
            _driver.Navigate().GoToUrl("https://localhost:5001/Movies");
            var oriMovieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));
            _driver.FindElement(By.LinkText("Delete")).Click();

            _driver.FindElement(By.LinkText("Back to List")).Click();

            var movieList = _driver.FindElements(By.CssSelector("[class='table'] tbody tr"));

            Assert.AreEqual("Index - Movie App", _driver.Title);
            Assert.AreEqual(oriMovieList.Count, movieList.Count);
        }


    }
}
