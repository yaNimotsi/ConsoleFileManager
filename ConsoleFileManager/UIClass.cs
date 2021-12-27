using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
namespace ConsoleFileManager
{
    class UIClass
    {
        private readonly string _firstPath;
        private readonly string _userLastPath;
        public int CountRowToView { get; }
        public string JournalPath { get; }

        private List<string> buffer;
        private int numPage = 0;

        private FilesClass _filesClass;
        private FoldersClasses _foldersClasses;

        public UIClass()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 41);
            Console.SetBufferSize(120, 41);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Title = "ConsoleFileManager";

            ReadAppSettings();

            PrintTitle();
            PrintHorizontalBorder(1);

            if (_userLastPath.Length > 0)
                GetContent(_userLastPath);
            else
                GetContent(_firstPath);
        }

        /// <summary>
        /// Чтение и назначение полям класса первичных настроек приложения
        /// </summary>
        private void ReadAppSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.Count == 0)
            {
                Console.WriteLine("AppSetting is empty");
            }
            else
            {
                _firstPath = appSettings["FirstPath"];
                _userLastPath = appSettings["LastUserPath"];
                CountRowToView = Convert.ToInt32(appSettings["CountRowToView"]);
                JournalPath = appSettings["JournalPath"];
            }

            PrintTitle();
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
        /// <param name="folderPath"> Путь к текущей папке</param>
        private void GetContent(string folderPath)
        {
            int rCounter = 2;

            DirectoryInfo directoryInfo = new DirectoryInfo(@"" + folderPath);

            //Печать информации о папках
            PrintInfoAboutContentFolders(directoryInfo.GetDirectories(), ref rCounter);

            //Печать информации о файлах
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            PrintInfoAboutContentFiles(fileInfos, ref rCounter);
            
            Console.ReadLine();
        }

        #region PrintInfoAboutContent
        /// <summary>
        /// Метод вывода информации о подпапках работающий с классом FoldersClass
        /// </summary>
        /// <param name="dInfo"> Массив объектов о подпапках</param>
        /// <param name="rCounter"> Строка для вставки</param>
        private void PrintInfoAboutContentFolders2(DirectoryInfo[] dInfo, ref int rCounter)
        {
            _foldersClasses = new FoldersClasses(dInfo);

            foreach (var folder in _foldersClasses.ListFolderClass)
            {
                Console.SetCursorPosition(3, rCounter);
                Console.WriteLine(folder.FolderName);

                Console.SetCursorPosition(60, rCounter);
                Console.WriteLine("Папка");

                rCounter++;
            }
        }

        /// <summary>
        /// Вывод информации о файлах работая через собственный класс файлов
        /// </summary>
        /// <param name="fInfos"></param>
        /// <param name="rCounter"></param>
        private void PrintInfoAboutContentFiles2(FileInfo[] fInfos, ref int rCounter)
        {
            _filesClass = new FilesClass(fInfos);

            foreach (var fileInFolder in _filesClass.Files)
            {
                Console.SetCursorPosition(3, rCounter);
                Console.WriteLine(fileInFolder.NameFile);
                
                Console.SetCursorPosition(60, rCounter);
                Console.WriteLine(fileInFolder.FileExtension);

                Console.SetCursorPosition(72, rCounter);
                Console.WriteLine(fileInFolder.FileAccesType);

                Console.SetCursorPosition(87, rCounter);
                Console.WriteLine(fileInFolder.DateCreate);

                Console.SetCursorPosition(105, rCounter);
                Console.WriteLine(fileInFolder.FileSyze);

                rCounter++;
            }
        }
        #endregion

        #region defaultPrint
        /// <summary>
        /// Печать заголовков столбцов
        /// </summary>
        private void PrintTitle()
        {
            Console.SetCursorPosition(3, 0);
            Console.Write("Имя");
            PrintVerticalBorder(58);

            Console.SetCursorPosition(60, 0);
            Console.Write("Тип");
            PrintVerticalBorder(70);

            Console.SetCursorPosition(72, 0);
            Console.Write("Доступ");
            PrintVerticalBorder(84);

            Console.SetCursorPosition(87, 0);
            Console.Write("Дата создания");
            PrintVerticalBorder(102);

            Console.SetCursorPosition(105, 0);
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
            for (var i = 0; i < 41; i++)
            {
                Console.SetCursorPosition(cNum, i);
                Console.Write('\u007C');
            }
        }
        #endregion
        /// <summary>
        /// Печать ограниченного кол-ва строк информации на странице
        /// </summary>
        private void PrintSectionContent()
        {
            if (buffer.Count == 0)
                return;

            int x = 0, y = 0;
            var sNum = numPage * _countRowToView;

            for (var i = sNum; i < (numPage + 1) * _countRowToView && i < buffer.Count; i++)
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
