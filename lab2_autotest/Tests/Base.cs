using NUnit.Framework;
using OpenQA.Selenium;
using Serilog;
using System;
using System.IO;

namespace lab2_autotest.Tests
{
    public class Base
    {
        public static ILogger Logger;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var logDirectory = "Logs";
            Directory.CreateDirectory(logDirectory);

            Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} ({ThreadId}) {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Combine(logDirectory, "test_log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    retainedFileCountLimit: 30,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} ({ThreadId}) {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        [SetUp]
        public void Setup()
        {
            string testName = TestContext.CurrentContext.Test.Name;

            Logger.Information("===== Starting test: {TestName} =====", testName);

            var builder = new WebDriverBuilder();
            WebDriverSingleton.Init(builder.Build());
            WebDriverSingleton.Driver.Manage().Window.Maximize();

            Logger.Debug("WebDriver initialized and window maximized");
        }

        [TearDown]
        public void Cleanup()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            var result = TestContext.CurrentContext.Result.Outcome.Status;

            if (result == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Logger.Error("Test {TestName} FAILED: {Message}", testName, TestContext.CurrentContext.Result.Message);
            }
            else if (result == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                Logger.Information("Test {TestName} PASSED", testName);
            }
            else
            {
                Logger.Warning("Test {TestName} ended with status: {Status}", testName, result);
            }

            Logger.Information("===== Ending test: {TestName} =====", testName);

            WebDriverSingleton.Quit();
            Logger.Debug("WebDriver quit");
        }
    }
}
