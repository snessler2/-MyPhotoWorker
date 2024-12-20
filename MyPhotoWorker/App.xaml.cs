using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.IO;

namespace MyPhotoWorker
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static string AppExePath = AppDomain.CurrentDomain.BaseDirectory;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Console.WriteLine("111111111111111111111111111111111111111111111111111");
            if (System.IO.Directory.Exists(@"c:\portableapps\myphotoworker\")) AppExePath = @"c:\portableapps\myphotoworker\";
            if (System.IO.Directory.Exists(@"d:\programme\myphotoworker_apple\")) AppExePath = @"d:\programme\myphotoworker_apple\";
            if (System.IO.Directory.Exists(@"d:\tools\myphotoworker\")) AppExePath = @"d:\tools\myphotoworker\";
        }

    }
}
