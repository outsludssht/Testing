using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Threading;
using Xunit;

namespace lab2_autotest
{
    [CollectionDefinition("Navigation Tests", DisableParallelization = false)]
    public class NavigationTestCollection : ICollectionFixture<NavigationTests>
    {
    }

    [Collection("Navigation Tests")]
    public class NavigationTests : IDisposable
    {
        private readonly IWebDriver driver;

        public NavigationTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        private IWebDriver GetDriver() => driver;

        [Fact(DisplayName = "[Navigation] Verify navigation to About page")]
        public void VerifyNavigationToAboutPage()
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement aboutLink = GetDriver().FindElement(By.XPath("//a[text()='About']"));
            aboutLink.Click();

            Thread.Sleep(300);

            Assert.Equal("https://en.ehu.lt/about/", GetDriver().Url);
            Assert.Equal("About", GetDriver().Title);

            IWebElement header = GetDriver().FindElement(By.XPath("//h1[@class='subheader__title']"));
            Assert.Equal("About", header.Text);
        }

        public static TheoryData<string, string> SearchData => new TheoryData<string, string>
        {
            { "study programs", "https://en.ehu.lt/?s=study+programs" },
            { "faculty", "https://en.ehu.lt/?s=faculty" }
        };

        [Theory(DisplayName = "[Search] Verify search functionality")]
        [MemberData(nameof(SearchData))]
        public void VerifySearchFunctionality(string searchQuery, string expectedUrl)
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement searchTrigger = GetDriver().FindElement(By.CssSelector(".header-search"));
            Actions actions = new Actions(GetDriver());
            actions.MoveToElement(searchTrigger).Perform();

            Thread.Sleep(300);

            IWebElement searchBox = GetDriver().FindElement(By.Name("s"));
            searchBox.SendKeys(searchQuery);
            searchBox.SendKeys(Keys.Enter);

            Thread.Sleep(2000);

            Assert.Equal(expectedUrl, GetDriver().Url);

            IWebElement header = GetDriver().FindElement(By.XPath("//h1[@class='subheader__title']"));
            Assert.Equal("Search results", header.Text);
        }

        [Fact(DisplayName = "[Localization] Verify language switching")]
        public void VerifyLanguage()
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement searchTrigger = GetDriver().FindElement(By.CssSelector(".language-switcher"));
            Actions actions = new Actions(GetDriver());
            actions.MoveToElement(searchTrigger).Perform();

            Thread.Sleep(300);

            IWebElement LTlanguage = GetDriver().FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']"));
            LTlanguage.Click();

            Thread.Sleep(2000);

            Assert.Equal("https://lt.ehu.lt/", GetDriver().Url);
        }

        [Fact(DisplayName = "[Content] Verify contact form contents")]
        public void VerifyContactForm()
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/contact");

            IWebElement email = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='35448735101']"));
            Assert.Equal("E-mail: franciskscarynacr@gmail.com", email.Text);

            IWebElement phoneLT = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='50296369501']"));
            Assert.Equal("Phone (LT): +370 68 771365", phoneLT.Text);

            IWebElement phoneBY = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='39744896801']"));
            Assert.Equal("Phone (BY): +375 29 5781488", phoneBY.Text);

            IWebElement networks = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='64965466401']"));
            Assert.Equal("Join us in the social networks: Facebook Telegram VK", networks.Text);
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}