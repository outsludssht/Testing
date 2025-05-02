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

        private By EmailLocator => By.XPath("//li[@plerdy-tracking-id ='35448735101']");
        private By PhoneLTLocator => By.XPath("//li[@plerdy-tracking-id ='50296369501']");
        private By PhoneBYLocator => By.XPath("//li[@plerdy-tracking-id ='39744896801']");
        private By SocialTextLocator => By.XPath("//li[@plerdy-tracking-id ='64965466401']");

        public void Open() => driver.Navigate().GoToUrl("https://en.ehu.lt/contact");
        public string GetEmail() => driver.FindElement(EmailLocator).Text;
        public string GetPhoneLT() => driver.FindElement(PhoneLTLocator).Text;
        public string GetPhoneBY() => driver.FindElement(PhoneBYLocator).Text;
        public string GetSocialText() => driver.FindElement(SocialTextLocator).Text;
    }

}