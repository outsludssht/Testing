using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using lab2_autotest.WebDriver;

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
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var searchIcon = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SearchIconLocator));
                searchIcon.Click();     
                searchBox.Clear();
                searchBox.SendKeys(query);
                searchBox.Submit();
                wait.Until(driver => driver.Url.Contains("?s="));
            }
            catch (Exception ex)
            {
                StepDefinitions.ReqnrollHooks.Logger.Warning("Exception during search: {Exception}. Using fallback approach.", ex.Message);
                
                new Actions(driver)
                    .MoveToElement(driver.FindElement(SearchIconLocator))
                    .Click()
                    .Pause(TimeSpan.FromSeconds(1))
                    .Perform();
                
                var searchBox = driver.FindElement(SearchInputLocator);
                searchBox.Clear();
                searchBox.SendKeys(query);
                searchBox.SendKeys(Keys.Enter);
                
                Thread.Sleep(2000);
            }
        }

        public void SwitchLanguageToLT()
        {
            new Actions(driver).MoveToElement(driver.FindElement(LanguageSwitcherLocator)).Perform();
            driver.FindElement(LithuanianLinkLocator).Click();
        }
    }
}