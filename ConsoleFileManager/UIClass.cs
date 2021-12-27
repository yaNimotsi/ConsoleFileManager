using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
namespace ConsoleFileManager
{
    internal class UIClass
    {
        private string _firstPath;
        private string _userLastPath;
        private int _countRowToView;
        private static int _rowsToTopMargin;
        private const int _rowToCommand = 39;
        private const int _columnToCommand = 17;
        //public string JournalPath { get; set; }

        private IEnumerable<DirectoryInfo> _subFolders; 
        private IEnumerable<FileInfo> _subFiles; 

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

            StartSettings();
        }

        /// <summary>
        /// Чтение и назначение полям класса первичных настроек приложения
        /// </summary>
        private void StartSettings()
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
                _countRowToView = Convert.ToInt32(appSettings["CountRowToView"]);
                //JournalPath = appSettings["JournalPath"];
            }

            _subFolders = FolderContents.GetSubFolders(_firstPath);
            _subFiles = FolderContents.GetSubFiles(_firstPath);

            PrintTable();

            var a = 2;

            PrintInfoAboutContentFolders(ref a);
            PrintInfoAboutContentFiles(ref a);
            Console.SetCursorPosition(_columnToCommand, _rowToCommand);

            //GetContent(_userLastPath.Length > 0 ? _userLastPath : _firstPath);
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

        
        private void PrintInfoAboutContentFolders(ref int rCounter)
        {
            foreach (var folder in _subFolders)
            {
                Console.SetCursorPosition(3, rCounter);
                Console.WriteLine(folder.Name);

                Console.SetCursorPosition(60, rCounter);
                Console.WriteLine("Папка");

                rCounter++;
            }
        }

        private void PrintInfoAboutContentFiles(ref int rCounter)
        {
            foreach (var fileInFolder in _subFiles)
            {
                Console.SetCursorPosition(3, rCounter);
                var nameFile = fileInFolder.Name;
                if (nameFile.Length > 50)
                {
                    nameFile = nameFile.Remove(50) + "...";
                }
                Console.WriteLine(nameFile);

                Console.SetCursorPosition(60, rCounter);
                Console.WriteLine(fileInFolder.Extension);

                Console.SetCursorPosition(72, rCounter);

                if ((fileInFolder.Attributes & FileAttributes.ReadOnly) != 0)
                {
                    Console.WriteLine("ReadOnly");
                }

                //Console.WriteLine((fileInFolder.Attributes & FileAttributes.ReadOnly) != 0  );

                Console.SetCursorPosition(85, rCounter);
                Console.WriteLine(fileInFolder.CreationTime);

                Console.SetCursorPosition(108, rCounter);
                Console.WriteLine(fileInFolder.Length);

                rCounter++;
            }
        }

        #region printTableBorder

        private static void PrintTable()
        {
            if (_rowsToTopMargin == 0) _rowsToTopMargin = 1;

            PrintTitle(_rowsToTopMargin - 1);
            PrintHorizontalBorder(_rowsToTopMargin);
        }

        /// <summary>
        /// Печать заголовков столбцов
        /// </summary>
        private static void PrintTitle(int rowToPrintHeader)
        {
            Console.SetCursorPosition(3, rowToPrintHeader);
            Console.Write("Имя");
            PrintVerticalBorder(58);

            Console.SetCursorPosition(60, rowToPrintHeader);
            Console.Write("Тип");
            PrintVerticalBorder(70);

            Console.SetCursorPosition(72, rowToPrintHeader);
            Console.Write("Доступ");
            PrintVerticalBorder(84);

            Console.SetCursorPosition(87, rowToPrintHeader);
            Console.Write("Дата создания");
            PrintVerticalBorder(105);

            Console.SetCursorPosition(108, rowToPrintHeader);
            Console.Write("Размер");

            Console.SetCursorPosition(0, _rowToCommand);
            Console.Write("Введите команду:");
        }

        /// <summary>
        /// Печать горинзонтальных линий
        /// </summary>
        /// <param name="rowsToTopMargin"></param>
        private static void PrintHorizontalBorder(int rowsToTopMargin)
        {
            for (var i = 0; i < 120; i++)
            {
                Console.SetCursorPosition(i, rowsToTopMargin);
                Console.Write('\u2014');
                Console.SetCursorPosition(i, _rowToCommand - 1);
                Console.Write('\u2014');
            }
        }

        /// <summary>
        /// Печать вертикальных линий
        /// </summary>
        /// <param name="cNum"></param>
        private static void PrintVerticalBorder(int cNum)
        {
            for (var i = 0; i < 39; i++)
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
