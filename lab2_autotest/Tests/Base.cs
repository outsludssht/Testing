using lab2_autotest.WebDriver;
using NUnit.Allure.Core;
using Allure.Commons;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Allure.Commons;

public class Base
{
    [SetUp]
    public void Setup()
    {
        AddEnvironmentInfo();
        var builder = new WebDriverBuilder();
        WebDriverSingleton.Init(builder.Build());
        WebDriverSingleton.Driver.Manage().Window.Maximize();
    }

    [TearDown]
    public void TearDown()
    {
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        var rawName = TestContext.CurrentContext.Test.Name;
        var safeName = string.Concat(rawName.Split(Path.GetInvalidFileNameChars()));

        string customMessage = outcome == TestStatus.Failed
            ? "Test failed! Check the screenshot."
            : "Test passed successfully.";

        try
        {
            var messageFilePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{safeName}_message.txt");
            File.WriteAllText(messageFilePath, customMessage);

            AllureLifecycle.Instance.AddAttachment("Custom Message", "text/plain", messageFilePath);

            TestContext.Progress.WriteLine($"Test '{rawName}' finished with status: {outcome}. {customMessage}");

            if (outcome == TestStatus.Failed)
            {
                var screenshot = ((ITakesScreenshot)WebDriverSingleton.Driver).GetScreenshot();
                var screenshotDir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                Directory.CreateDirectory(screenshotDir);
                var screenshotPath = Path.Combine(screenshotDir, $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                screenshot.SaveAsFile(screenshotPath);

                AllureLifecycle.Instance.AddAttachment("Screenshot", "image/png", screenshotPath);
            }
        }
        catch (Exception ex)
        {
            TestContext.Progress.WriteLine($"[TearDown ERROR] {ex.Message}");
        }
        finally
        {
            WebDriverSingleton.Quit();
        }
    }



    private void AddEnvironmentInfo()
    {
        var environmentInfo = new Dictionary<string, string>
        {
            { "OS", Environment.OSVersion.ToString() },
            { ".NET Version", Environment.Version.ToString() },
            { "Test Start Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
        };

        foreach (var info in environmentInfo)
        {
            try
            {
                TestContext.Progress.WriteLine($"{info.Key}: {info.Value}");
                AllureLifecycle.Instance.AddAttachment(info.Key, "text/plain", info.Value);
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error adding environment info: {ex.Message}");
            }
        }
    }
}
