using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest.Pages
{
    internal class ContactPage
    {
        private readonly IWebDriver driver = WebDriverSingleton.Driver;

        public void Open() => driver.Navigate().GoToUrl("https://en.ehu.lt/contact");

        public string GetEmail() =>
            driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='35448735101']")).Text;

        public string GetPhoneLT() =>
            driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='50296369501']")).Text;

        public string GetPhoneBY() =>
            driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='39744896801']")).Text;

        public string GetSocialText() =>
            driver.FindElement(By.XPath("//li[@plerdy-tracking-id ='64965466401']")).Text;
    }
}
