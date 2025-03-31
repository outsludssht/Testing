using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;

namespace lab2_autotest
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class NavigationTests
    {
        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        [SetUp]
        public void Setup()
        {
            driver.Value = new ChromeDriver();
            driver.Value.Manage().Window.Maximize();
        }

        IWebDriver GetDriver() => driver.Value;

        [Test, Category("Navigation")]
        public void VerifyNavigationToAboutPage()
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement aboutLink = GetDriver().FindElement(By.XPath("//a[text()='About']"));
            aboutLink.Click();

            System.Threading.Thread.Sleep(300);

            Assert.That(GetDriver().Url, Is.EqualTo("https://en.ehu.lt/about/"), "URL не совпал.");

            // у меня на сайте нет заголовка, который у вас в примере, поэтому вставила новый
            Assert.That(GetDriver().Title, Is.EqualTo("About"), "Заголовок страницы не тот.");

            IWebElement header = GetDriver().FindElement(By.XPath("//h1[@class='subheader__title']"));
            Assert.That(header.Text, Is.EqualTo("About"), "Заголовок контента не совпадает.");
        }

        [Test, Category("Search")]
        [TestCase("study programs", "https://en.ehu.lt/?s=study+programs")]
        [TestCase("faculty", "https://en.ehu.lt/?s=faculty")]
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

            Assert.That(GetDriver().Url, Is.EqualTo(expectedUrl), "URL не совпал.");

            IWebElement header = GetDriver().FindElement(By.XPath("//h1[@class='subheader__title']"));
            Assert.That(header.Text, Is.EqualTo("Search results"), "Заголовок контента неверный!");
        }

        [Test, Category("Localization")]
        public void VerifyLanguage()
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement searchTrigger = GetDriver().FindElement(By.CssSelector(".language-switcher"));

            Actions actions = new Actions(GetDriver());
            actions.MoveToElement(searchTrigger).Perform();

            System.Threading.Thread.Sleep(300);

            IWebElement LTlanguage = GetDriver().FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']"));

            LTlanguage.Click();

            System.Threading.Thread.Sleep(2000);

            Assert.That(GetDriver().Url, Is.EqualTo("https://lt.ehu.lt/"), "URL не совпал.");
        }

        [Test, Category("Content")]
        public void VerifyContactForm()
        {
            GetDriver().Navigate().GoToUrl("https://en.ehu.lt/contact");

            IWebElement email = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='35448735101']"));
            Assert.That(email.Text, Is.EqualTo("E-mail: franciskscarynacr@gmail.com"), "Неверная почта.");

            IWebElement phoneLT = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='50296369501']"));
            Assert.That(phoneLT.Text, Is.EqualTo("Phone (LT): +370 68 771365"), "Содержание LT телефона не совпало.");

            IWebElement phoneBY = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='39744896801']"));
            Assert.That(phoneBY.Text, Is.EqualTo("Phone (BY): +375 29 5781488"), "Содержание BY телефона не совпало.");

            IWebElement networks = GetDriver().FindElement(By.XPath("//li[@plerdy-tracking-id ='64965466401']"));
            Assert.That(networks.Text, Is.EqualTo("Join us in the social networks: Facebook Telegram VK"), "Содержание контента не совпало.");
        }


        [TearDown]
        public void Cleanup()
        {
            if (driver.Value != null)
            {
                driver.Value.Quit();
                driver.Value.Dispose();
            }
        }
    }
}
