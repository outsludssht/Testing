using Reqnroll;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(1)]

namespace lab2_autotest
{
    [Binding]
    public class ReqnrollConfig
    {
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            StepDefinitions.ReqnrollHooks.Logger.Information("Starting Feature: {FeatureTitle}", featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            StepDefinitions.ReqnrollHooks.Logger.Information("Completed Feature: {FeatureTitle}", featureContext.FeatureInfo.Title);
        }
    }
} 