using lab2_autotest.WebDriver;
using Reqnroll;
using Serilog;
using System.IO;

namespace lab2_autotest.StepDefinitions
{
    [Binding]
    public class ReqnrollHooks
    {
        private readonly ScenarioContext _scenarioContext;
        public static ILogger Logger;

        public ReqnrollHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var logDirectory = "Logs";
            Directory.CreateDirectory(logDirectory);

            Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} ({ThreadId}) {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Combine(logDirectory, "reqnroll_log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    retainedFileCountLimit: 30,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} ({ThreadId}) {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Logger.Information("Starting test execution");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Logger.Information("===== Starting scenario: {ScenarioTitle} =====", _scenarioContext.ScenarioInfo.Title);

            var builder = new WebDriverBuilder();
            WebDriverSingleton.Init(builder.Build());
            WebDriverSingleton.Driver.Manage().Window.Maximize();

            Logger.Debug("WebDriver initialized and window maximized");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var scenarioTitle = _scenarioContext.ScenarioInfo.Title;
            
            if (_scenarioContext.TestError != null)
            {
                Logger.Error("Scenario {ScenarioTitle} FAILED: {ErrorMessage}", 
                    scenarioTitle, 
                    _scenarioContext.TestError.Message);
            }
            else
            {
                Logger.Information("Scenario {ScenarioTitle} PASSED", scenarioTitle);
            }

            Logger.Information("===== Ending scenario: {ScenarioTitle} =====", scenarioTitle);

            WebDriverSingleton.Quit();
            Logger.Debug("WebDriver quit");
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Logger.Information("Test execution completed");
        }
    }
} 