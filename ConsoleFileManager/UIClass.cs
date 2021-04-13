using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace ConsoleFileManager
{
    class UIClass
    {
        private string _firstPath;
        private string _userLastPath;
        private int _countRowToView;
        private string _journalPath;

        public UIClass()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 41);
            Console.SetBufferSize(120, 41);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;



            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.Count == 0)
            {
                Console.WriteLine("AppSetting is empty");
            }
            else
            {
                _firstPath = appSettings["FirstPath"];
                _userLastPath = appSettings["LastUserPath"];
                _countRowToView = Convert.ToInt32(appSettings["CountRowToView"]);
                _journalPath = appSettings["JournalPath"];
                var asd = appSettings["NewSetting"];

            }


            UpdateAppConfigFile("LastUserPath", @"C\");
            UpdateAppConfigFile("NewSetting", @"C\");



            






            Assembly localisationAssembly = Assembly.Load("ConsoleFileManager");
            ResourceManager resourceManager = new ResourceManager("ConsoleFileManager.Properties.Resources", localisationAssembly);

            //ResourceManager resourceManager = new ResourceManager("Resources.resx", typeof(UIClass).Assembly);

            _firstPath = resourceManager.GetString("FristPath");
            Console.WriteLine(_firstPath);
            _userLastPath = resourceManager.GetString("UserLastPath");
            Console.WriteLine(_userLastPath);

            Console.WriteLine();

            DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\GeekBrains\Base C#\Algorimts\AlgoritmsLesson6");

            foreach (var dirInf in directoryInfo.GetDirectories())
            {
                Console.WriteLine(dirInf.Name);
            }

            foreach(var dirInf in directoryInfo.GetFiles())
            {
                Console.WriteLine(dirInf.Name);
            }
            Console.ReadLine();
        }

        private void UpdateAppConfigFile(string key,string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                ConfigurationManager.RefreshSection(configFile.ConnectionStrings.SectionInformation.SectionName);
                configFile.Save(ConfigurationSaveMode.Modified);
                //ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
    }
}
