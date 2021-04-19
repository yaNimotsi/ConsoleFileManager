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

        private List<string> buffer;
        private int numPage = 0;

        public UIClass()
        {
            Console.CursorVisible = false;
            
            Console.SetWindowSize(120, 60);
            Console.SetBufferSize(120, 60);
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Title = "ConsoleFileManager";

            ReadAppSettings();

            if (_userLastPath.Length > 0)
                PrintFolderContent(_userLastPath);
            else
                PrintFolderContent(_firstPath);
        }

        /// <summary>
        /// Чтение и назначение полям класса первичных настроек приложения
        /// </summary>
        private void ReadAppSettings()
        {
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = appSettings.AppSettings.Settings;
            if (settings.Count == 0)
            {
                Console.WriteLine("AppSetting is empty");
            }
            else
            {
                foreach (KeyValueConfigurationElement key in settings)
                {
                    switch (key.Key)
                    {
                        case "FirstPath":
                            _firstPath = key.Value;
                            break;
                        case "LastUserPath":
                            _userLastPath = key.Value;
                            break;
                        case "CountRowToView":
                            _countRowToView = Convert.ToInt32(key.Value);
                            break;
                        case "JournalPath":
                            _journalPath = key.Value;
                            break;
                    };

                }

            }
        }

        /// <summary>
        /// Обновление либо добавление параметра в файл конфигурации
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void UpdateAppConfigFile(string key, string value)
        {
            try
            {
                //Получение файла конфигурации исполняемого файла
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
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Обновить или добавить параметр в файл конфигурации не удалось!");
            }
        }

        /// <summary>
        /// Печать содержимого папки
        /// </summary>
        /// <param name="folderPath">Адрес папки, содержимое которой необходимо отобразить</param>
        private void PrintFolderContent(string folderPath)
        { 
            Console.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(@"" + folderPath);

            buffer = new List<string>();
            
            foreach (var dirInf in directoryInfo.GetDirectories())
            {
                buffer.Add(dirInf.Name);
            }
            foreach (var dirInf in directoryInfo.GetFiles())
            {
                buffer.Add(dirInf.Name);
            }

            PrintSectionContent();

            Console.ReadLine();
        }

        /// <summary>
        /// Печать ограниченного кол-ва строк информации на странице
        /// </summary>
        private void PrintSectionContent()
        {
            if (buffer.Count == 0)
                return;

            int x = 0, y = 0;
            int sNum = numPage * _countRowToView;

            for (int i = sNum; i < (numPage + 1) * _countRowToView && i < buffer.Count; i++)
            {
                Console.SetCursorPosition(x + 3, y + 2);
                Console.WriteLine(buffer[i]);
                y++;
            }

            //Увеличение значения страницы с содержимым, которая отображена
            numPage++;
        }
    }
}
