using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using lab2_autotest.Pages;

namespace lab2_autotest.Tests
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class NavigationTests : Base
    {
        [Test, Category("Navigation")]
        public void VerifyNavigationToAboutPage()
        {
            Logger.Information("Starting test: VerifyNavigationToAboutPage");

            var main = new MainPage();
            main.Open();
            Logger.Debug("Navigated to MainPage");

            main.ClickAbout();
            Logger.Debug("Clicked on About link");

            var currentUrl = WebDriverSingleton.Driver.Url;
            var headerText = main.GetHeaderText();

            currentUrl.Should().Be("https://en.ehu.lt/about/", "URL should match the About page URL");
            headerText.Should().Be("About", "Header text should be 'About'");

            Logger.Information("Test VerifyNavigationToAboutPage completed");
        }

        [TestCase("study programs", "https://en.ehu.lt/?s=study+programs")]
        [TestCase("faculty", "https://en.ehu.lt/?s=faculty")]
        [Test, Category("Search")]
        public void VerifySearchFunctionality(string query, string expectedUrl)
        {
            Logger.Information("Starting test: VerifySearchFunctionality with query: {Query}", query);

            var main = new MainPage();
            main.Open();
            Logger.Debug("Navigated to MainPage");

            main.Search(query);
            Logger.Debug("Performed search with query: {Query}", query);

            var currentUrl = WebDriverSingleton.Driver.Url;
            currentUrl.Should().Be(expectedUrl, "URL should match the expected search results URL");

            Logger.Information("Test VerifySearchFunctionality passed for query: {Query}", query);
        }

        [Test, Category("Localization")]
        public void VerifyLanguage()
        {
            Logger.Information("Starting test: VerifyLanguage");

            var main = new MainPage();
            main.Open();
            Logger.Debug("Navigated to MainPage");

            main.SwitchLanguageToLT();
            Logger.Debug("Switched language to LT");

            WebDriverSingleton.Driver.Url.Should().Be("https://lt.ehu.lt/", "URL should match the Lithuanian language page");

            Logger.Information("Test VerifyLanguage passed");
        }

        [Test, Category("Content")]
        public void VerifyContactForm()
        {
            Logger.Information("Starting test: VerifyContactForm");

            var contact = new ContactPage();
            contact.Open();
            Logger.Debug("Navigated to Contact page");

            contact.GetEmail().Should().Be("E-mail: franciskscarynacr@gmail.com", "Email should match the expected value");
            contact.GetPhoneLT().Should().Be("Phone (LT): +370 68 771365", "Phone (LT) should match the expected value");
            contact.GetPhoneBY().Should().Be("Phone (BY): +375 29 5781488", "Phone (BY) should match the expected value");
            contact.GetSocialText().Should().Be("Join us in the social networks: Facebook Telegram VK", "Social text should match the expected value");

            Logger.Information("Test VerifyContactForm passed");
        }
    }
}