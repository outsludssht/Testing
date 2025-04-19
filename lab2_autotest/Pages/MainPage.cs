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
        private By AboutLinkLocator => By.XPath("//a[text()='About']");
        private By HeaderLocator => By.XPath("//h1[@class='subheader__title']");
        private By SearchIconLocator => By.CssSelector(".header-search");
        private By SearchInputLocator => By.Name("s");
        private By LanguageSwitcherLocator => By.CssSelector(".language-switcher");
        private By LithuanianLinkLocator => By.XPath("//a[@href='https://lt.ehu.lt/']");

        public void Open() => driver.Navigate().GoToUrl("https://en.ehu.lt/");
        public void ClickAbout() => driver.FindElement(AboutLinkLocator).Click();
        public string GetHeaderText() => driver.FindElement(HeaderLocator).Text;

        public void Search(string query)
        {
            new Actions(driver).MoveToElement(driver.FindElement(SearchIconLocator)).Perform();
            var searchBox = driver.FindElement(SearchInputLocator);
            searchBox.SendKeys(query);
            searchBox.SendKeys(Keys.Enter);
        }

        public void SwitchLanguageToLT()
        {
            new Actions(driver).MoveToElement(driver.FindElement(LanguageSwitcherLocator)).Perform();
            driver.FindElement(LithuanianLinkLocator).Click();
        }
    }
}
