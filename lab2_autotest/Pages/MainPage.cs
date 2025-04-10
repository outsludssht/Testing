using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest.Pages
{
    internal class MainPage
    {
        private readonly IWebDriver driver = WebDriverSingleton.Driver;

        public void Open() => driver.Navigate().GoToUrl("https://en.ehu.lt/");

        public void ClickAbout()
        {
            var aboutLink = driver.FindElement(By.XPath("//a[text()='About']"));
            aboutLink.Click();
        }
        public string GetHeaderText()
        {
            return driver.FindElement(By.XPath("//h1[@class='subheader__title']")).Text;
        }

        public void Search(string query)
        {
            new Actions(driver).MoveToElement(driver.FindElement(By.CssSelector(".header-search"))).Perform();
            var box = driver.FindElement(By.Name("s"));
            box.SendKeys(query);
            box.SendKeys(Keys.Enter);
        }

        public void SwitchLanguageToLT()
        {
            new Actions(driver).MoveToElement(driver.FindElement(By.CssSelector(".language-switcher"))).Perform();
            driver.FindElement(By.XPath("//a[@href='https://lt.ehu.lt/']")).Click();
        }
    }
}
