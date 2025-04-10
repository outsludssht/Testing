using lab2_autotest.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;

namespace lab2_autotest.Tests
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class NavigationTests : Base
    {
        [Test, Category("Navigation")]
        public void VerifyNavigationToAboutPage()
        {
            var main = new MainPage();
            main.Open();
            main.ClickAbout();

            Assert.That(WebDriverSingleton.Driver.Url, Is.EqualTo("https://en.ehu.lt/about/"));
            Assert.That(WebDriverSingleton.Driver.Title, Is.EqualTo("About"));
            Assert.That(main.GetHeaderText(), Is.EqualTo("About"));
        }

        [TestCase("study programs", "https://en.ehu.lt/?s=study+programs")]
        [TestCase("faculty", "https://en.ehu.lt/?s=faculty")]
        [Test, Category("Search")]
        public void VerifySearchFunctionality(string query, string expectedUrl)
        {
            var main = new MainPage();
            main.Open();
            main.Search(query);

            Assert.That(WebDriverSingleton.Driver.Url, Is.EqualTo(expectedUrl));
            Assert.That(main.GetHeaderText(), Is.EqualTo("Search results"));
        }

        [Test, Category("Localization")]
        public void VerifyLanguage()
        {
            var main = new MainPage();
            main.Open();
            main.SwitchLanguageToLT();

            Assert.That(WebDriverSingleton.Driver.Url, Is.EqualTo("https://lt.ehu.lt/"));
        }

        [Test, Category("Content")]
        public void VerifyContactForm()
        {
            var contact = new ContactPage();
            contact.Open();

            Assert.That(contact.GetEmail(), Is.EqualTo("E-mail: franciskscarynacr@gmail.com"));
            Assert.That(contact.GetPhoneLT(), Is.EqualTo("Phone (LT): +370 68 771365"));
            Assert.That(contact.GetPhoneBY(), Is.EqualTo("Phone (BY): +375 29 5781488"));
            Assert.That(contact.GetSocialText(), Is.EqualTo("Join us in the social networks: Facebook Telegram VK"));
        }
    }
}