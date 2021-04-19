﻿using System;
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
                _firstPath = appSettings["FirstPath"];
                _userLastPath = appSettings["LastUserPath"];
                _countRowToView = Convert.ToInt32(appSettings["CountRowToView"]);
                _journalPath = appSettings["JournalPath"];
            }

            PrintTytle();
            PrintHorizontalBorder(1);

            if (_userLastPath.Length > 0)
                GetContent(_userLastPath);
            else
                GetContent(_firstPath);
        }

        #region UpadteAppConfigFile
        /// <summary>
        /// Метод, для обновления файла конфигурации
        /// </summary>
        /// <param name="key"> Имя параметра</param>
        /// <param name="value"> Знеачение параметра</param>
        private void UpdateAppConfigFile(string key, string value)
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
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
        #endregion
        
        /// <summary>
        /// Получение первичной информации о содержимом текущей папки
        /// </summary>
        /// <param name="FolderPath"> Путь к текущей папке</param>
        private void GetContent(string FolderPath)
        {
            int rCounter = 2;

            DirectoryInfo directoryInfo = new DirectoryInfo(FolderPath);

            //Печать информации о папках
            PrintInfoAboutContentFolders(directoryInfo.GetDirectories(), ref rCounter);

            //Печать информации о файлах
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            PrintInfoAboutContentFiles(fileInfos, ref rCounter);
            
            Console.ReadLine();
        }

        #region PrintInfoAboutContent
        /// <summary>
        /// Вывод информации о подпапках в текущей папке
        /// </summary>
        /// <param name="dInfo"> Массив содержащий информацию о подпапках</param>
        /// <param name="rCounter"> Строка для вставки</param>
        private void PrintInfoAboutContentFolders(DirectoryInfo[] dInfo, ref int rCounter)
        {
            foreach (var dirInf in dInfo)
            {
                Console.SetCursorPosition(3, rCounter);
                Console.WriteLine(dirInf.Name);

                Console.SetCursorPosition(87, rCounter);
                Console.WriteLine(dirInf.CreationTime.ToShortDateString());

                Console.SetCursorPosition(72, rCounter);
                Console.WriteLine("Папка");

                rCounter++;
            }
        }

        /// <summary>
        /// Вывод информации о файлах в текущей папке
        /// </summary>
        /// <param name="fInfos"> Массив содержащий информацию о файлах</param>
        /// <param name="rCounter"> Строка для вставки</param>
        private void PrintInfoAboutContentFiles(FileInfo[] fInfos, ref int rCounter)
        {
            foreach (var dirInf in fInfos)
            {
                Console.SetCursorPosition(3, rCounter);
                Console.WriteLine(dirInf.Name);

                Console.SetCursorPosition(87, rCounter);
                Console.WriteLine(dirInf.CreationTime.ToShortDateString());


                Console.SetCursorPosition(72, rCounter);
                Console.WriteLine(dirInf.Extension);

                Console.SetCursorPosition(105, rCounter);
                Console.WriteLine(dirInf.Length);

                rCounter++;
            }
        }
        #endregion

        #region defaultPrint
        /// <summary>
        /// Печать заголовков столбцов
        /// </summary>
        private void PrintTytle()
        {
            Console.SetCursorPosition(3, 0);
            Console.Write("Имя");
            PrintVerticalBorder(69);

            Console.SetCursorPosition(73, 0);
            Console.Write("Тип");
            PrintVerticalBorder(84);

            Console.SetCursorPosition(88, 0);
            Console.Write("Дата создания");
            PrintVerticalBorder(102);

            Console.SetCursorPosition(106, 0);
            Console.Write("Размер");
        }

        /// <summary>
        /// Печать горинзонтальных линий
        /// </summary>
        /// <param name="rNum"></param>
        private void PrintHorizontalBorder(int rNum)
        {
            for (int i = 0; i < 120; i++)
            {
                Console.SetCursorPosition(i, rNum);
                Console.Write('\u2014');
            }
        }

        /// <summary>
        /// Печать вертикальных линий
        /// </summary>
        /// <param name="cNum"></param>
        private void PrintVerticalBorder(int cNum)
        {
            var a = '┆';
            for (int i = 0; i < 41; i++)
            {
                Console.SetCursorPosition(cNum, i);
                Console.Write('\u007C');
            }

            //Увеличение значения страницы с содержимым, которая отображена
            numPage++;
        }
        #endregion
    }
}
