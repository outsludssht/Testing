using lab2_autotest.Pages;
using lab2_autotest.WebDriver;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;

namespace lab2_autotest.Tests
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    [AllureNUnit]
    [AllureSuite("Navigation Tests")]
    public class NavigationTests : Base
    {
        [Test, Category("Navigation")]
        [AllureSeverity(Allure.Commons.SeverityLevel.normal)]
        [AllureDescription("This test should pass.")]
        public void VerifyNavigationToAboutPage()
        {
            var main = new MainPage();
            main.Open();
            main.ClickAbout();

            Assert.That(WebDriverSingleton.Driver.Url, Is.EqualTo(Urls.About));
            Assert.That(WebDriverSingleton.Driver.Title, Is.EqualTo("About"));
            Assert.That(main.GetHeaderText(), Is.EqualTo("About"));
        }

        [TestCase("study programs", Urls.Study)]
        [TestCase("faculty", Urls.Faculty)]
        [Test, Category("Search")]
        [AllureSeverity(Allure.Commons.SeverityLevel.critical)]
        [AllureDescription("This test should fail.")]
        public void VerifySearchFunctionality(string query, string expectedUrl)
        {
            var main = new MainPage();
            main.Open();
            main.Search(query);

            Assert.That(WebDriverSingleton.Driver.Url, Is.EqualTo(Urls.LanguageLT));
            Assert.That(main.GetHeaderText(), Is.EqualTo("Something"));
        }

        [Test, Category("Localization")]
        [AllureSeverity(Allure.Commons.SeverityLevel.minor)]
        [AllureDescription("This test should be skipped.")]
        public void VerifyLanguage()
        {
            Assert.Inconclusive("This test is intentionally skipped.");
        }

        [Test, Category("Content")]
        [AllureSeverity(Allure.Commons.SeverityLevel.normal)]
        [AllureDescription("This test should pass.")]
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