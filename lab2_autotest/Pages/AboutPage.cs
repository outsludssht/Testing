using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest.Pages
{
    internal class AboutPage
    {
        private readonly IWebDriver driver;
        private By HeaderLocator => By.XPath("//h1[@class='subheader__title']");

        public AboutPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Open() => driver.Navigate().GoToUrl("https://en.ehu.lt/about/");
        public string GetHeaderText() => driver.FindElement(HeaderLocator).Text;
    }
}