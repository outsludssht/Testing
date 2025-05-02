using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest.WebDriver
{
    public class WebDriverBuilder
    {
        public IWebDriver Build()
        {
            var options = new ChromeOptions();
            return new ChromeDriver(options);
        }
    }
}