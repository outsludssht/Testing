using FluentAssertions;
using lab2_autotest.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System.Threading;

namespace lab2_autotest.StepDefinitions
{
    [Binding]
    public class EHUWebsiteSteps
    {
        private readonly IWebDriver _driver = WebDriverSingleton.Driver;
        private readonly MainPage _mainPage = new MainPage();
        private readonly ContactPage _contactPage = new ContactPage();
        private readonly AboutPage _aboutPage;

        public EHUWebsiteSteps()
        {
            _aboutPage = new AboutPage(_driver);
        }

        #region Navigation Steps

        [Given(@"I am on the EHU main page")]
        public void GivenIAmOnTheEHUMainPage()
        {
            ReqnrollHooks.Logger.Debug("Navigating to EHU main page");
            _mainPage.Open();
        }

        [Given(@"I am on the EHU contact page")]
        public void GivenIAmOnTheEHUContactPage()
        {
            ReqnrollHooks.Logger.Debug("Navigating to EHU contact page");
            _contactPage.Open();
        }

        [When(@"I click on the About link")]
        public void WhenIClickOnTheAboutLink()
        {
            ReqnrollHooks.Logger.Debug("Clicking on About link");
            _mainPage.ClickAbout();
        }

        [Then(@"I should be redirected to the About page")]
        public void ThenIShouldBeRedirectedToTheAboutPage()
        {
            ReqnrollHooks.Logger.Debug("Verifying URL of About page");
            _driver.Url.Should().Be("https://en.ehu.lt/about/", "URL should match the About page URL");
        }

        [Then(@"the page header should display ""(.*)""")]
        public void ThenThePageHeaderShouldDisplay(string expectedHeader)
        {
            ReqnrollHooks.Logger.Debug("Verifying page header text");
            var headerText = _mainPage.GetHeaderText();
            headerText.Should().Be(expectedHeader, $"Header text should be '{expectedHeader}'");
        }

        #endregion

        #region Search Steps

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string query)
        {
            ReqnrollHooks.Logger.Debug("Searching for: {Query}", query);
            _mainPage.Search(query);
            
            // Allow time for the page to load
            Thread.Sleep(2000);
        }

        [Then(@"I should be redirected to the search results page with URL ""(.*)""")]
        public void ThenIShouldBeRedirectedToTheSearchResultsPageWithURL(string expectedUrl)
        {
            ReqnrollHooks.Logger.Debug("Verifying search results URL");
            
            // First check if URL contains '?s=' which indicates a search
            _driver.Url.Should().Contain("?s=", "URL should contain search parameter");
            
            // Then check if the URL contains the expected search term
            // We extract the search term from the expected URL by splitting on '='
            if (expectedUrl.Contains("="))
            {
                var searchTerm = expectedUrl.Split('=')[1];
                _driver.Url.Should().Contain(searchTerm, "URL should contain the search term");
            }
        }

        #endregion

        #region Language Steps

        [When(@"I switch the language to Lithuanian")]
        public void WhenISwitchTheLanguageToLithuanian()
        {
            ReqnrollHooks.Logger.Debug("Switching language to Lithuanian");
            _mainPage.SwitchLanguageToLT();
        }

        [Then(@"I should be redirected to the Lithuanian version of the website")]
        public void ThenIShouldBeRedirectedToTheLithuanianVersionOfTheWebsite()
        {
            ReqnrollHooks.Logger.Debug("Verifying Lithuanian website URL");
            _driver.Url.Should().Be("https://lt.ehu.lt/", "URL should match the Lithuanian language page");
        }

        #endregion

        #region Contact Steps

        [Then(@"the email information should be ""(.*)""")]
        public void ThenTheEmailInformationShouldBe(string expectedEmail)
        {
            ReqnrollHooks.Logger.Debug("Verifying email information");
            _contactPage.GetEmail().Should().Be(expectedEmail, "Email should match the expected value");
        }

        [Then(@"the Lithuanian phone number should be ""(.*)""")]
        public void ThenTheLithuanianPhoneNumberShouldBe(string expectedPhone)
        {
            ReqnrollHooks.Logger.Debug("Verifying Lithuanian phone number");
            _contactPage.GetPhoneLT().Should().Be(expectedPhone, "Phone (LT) should match the expected value");
        }

        [Then(@"the Belarusian phone number should be ""(.*)""")]
        public void ThenTheBelarusianPhoneNumberShouldBe(string expectedPhone)
        {
            ReqnrollHooks.Logger.Debug("Verifying Belarusian phone number");
            _contactPage.GetPhoneBY().Should().Be(expectedPhone, "Phone (BY) should match the expected value");
        }

        [Then(@"the social media text should include Facebook, Telegram and VK")]
        public void ThenTheSocialMediaTextShouldIncludeFacebookTelegramAndVK()
        {
            ReqnrollHooks.Logger.Debug("Verifying social media text");
            var socialText = _contactPage.GetSocialText();
            socialText.Should().Contain("Facebook", "Social text should contain Facebook");
            socialText.Should().Contain("Telegram", "Social text should contain Telegram");
            socialText.Should().Contain("VK", "Social text should contain VK");
        }

        #endregion
    }
} 