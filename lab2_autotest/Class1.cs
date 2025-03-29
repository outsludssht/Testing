using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;

namespace lab2_autotest
{
    [TestFixture]
    public class NavigationTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyNavigationToAboutPage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement aboutLink = driver.FindElement(By.XPath("//a[text()='About']"));
            aboutLink.Click();

            System.Threading.Thread.Sleep(300);

            Assert.That(driver.Url, Is.EqualTo("https://en.ehu.lt/about/"), "URL не совпал.");

            // у меня на сайте нет заголовка, который у вас в примере, поэтому вставила новый
            Assert.That(driver.Title, Is.EqualTo("About"), "Заголовок страницы не тот.");

            IWebElement header = driver.FindElement(By.XPath("//h1[@class='subheader__title']"));
            Assert.That(header.Text, Is.EqualTo("About"), "Заголовок контента не совпадает.");
        }

        [Test]
        public void VerifySearchFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement searchTrigger = driver.FindElement(By.CssSelector(".header-search"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(searchTrigger).Perform();

            System.Threading.Thread.Sleep(300);

            IWebElement searchBox = driver.FindElement(By.Name("s"));
            searchBox.SendKeys("study programs");

            searchBox.SendKeys(Keys.Enter);

            System.Threading.Thread.Sleep(2000);

            Assert.That(driver.Url, Is.EqualTo("https://en.ehu.lt/?s=study+programs"), "URL не совпал.");

            IWebElement header = driver.FindElement(By.XPath("//h1[@class='subheader__title']"));
            Assert.That(header.Text, Is.EqualTo("Search results"), "Заголовок контента неверный!");
        }

        [Test]
        public void VerifyLanguage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            IWebElement searchTrigger = driver.FindElement(By.CssSelector(".language-switcher"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(searchTrigger).Perform();

            System.Threading.Thread.Sleep(300);

            IWebElement LTlanguage = driver.FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']"));

            LTlanguage.Click();

            System.Threading.Thread.Sleep(2000);

            Assert.That(driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "URL не совпал.");
        }

        [Test]
        public void VerifyContactForm()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contact");

            IWebElement email = driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='35448735101']"));
            Assert.That(email.Text, Is.EqualTo("E-mail: franciskscarynacr@gmail.com"), "Неверная почта.");

            IWebElement phoneLT = driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='50296369501']"));
            Assert.That(phoneLT.Text, Is.EqualTo("Phone (LT): +370 68 771365"), "Содержание LT телефона не совпало.");

            IWebElement phoneBY = driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='39744896801']"));
            Assert.That(phoneBY.Text, Is.EqualTo("Phone (BY): +375 29 5781488"), "Содержание BY телефона не совпало.");

            IWebElement networks = driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='64965466401']"));
            Assert.That(networks.Text, Is.EqualTo("Join us in the social networks: Facebook Telegram VK"), "Содержание контента не совпало.");
        }


        [TearDown]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}
