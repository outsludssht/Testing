using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_autotest.Pages
{
    internal class Urls
    {
        public const string Main = "https://en.ehu.lt/";
        public const string About = "https://en.ehu.lt/about/";
        public const string LanguageLT = "https://lt.ehu.lt/";
        public const string Study = "https://en.ehu.lt/?s=study+programs";
        public const string Faculty = "https://en.ehu.lt/?s=faculty";

        public static string Search(string query)
        {
            return $"{Main}?s={Uri.EscapeDataString(query)}";
        }
    }
}
