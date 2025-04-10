using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest.Tests
{
    public class Base
    {
        [SetUp]
        public void Setup()
        {
            var builder = new WebDriverBuilder();
            WebDriverSingleton.Init(builder.Build());
            WebDriverSingleton.Driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void Cleanup()
        {
            WebDriverSingleton.Quit();
        }
    }
}
