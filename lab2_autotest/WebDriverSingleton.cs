using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest
{
    public class WebDriverSingleton
    {
        private static ThreadLocal<IWebDriver> driverTest = new ThreadLocal<IWebDriver>();

        public static IWebDriver Driver => driverTest.Value;

        public static void Init(IWebDriver driver)
        {
            driverTest.Value = driver;
        }

        public static void Quit()
        {
            driverTest.Value.Quit();
            driverTest.Value.Dispose();
        }
    }
}
