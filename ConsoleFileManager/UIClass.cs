using System;
using System.Collections.Generic;
using System.Configuration;
namespace ConsoleFileManager
{
    internal class UiClass
    {
        private const int RowToCommand = 39;
        private const int ColumnToCommand = 17;
        public const string WindowName = "ConsoleFileManager";

        private string _firstPath;
        private static string _userLastPath;

        private static int _rowsToTopMargin;

        //public string JournalPath { get; set; }
        
        private static List<SubElement> _content;
        private static List<SubElement> _contentToPrint;

        private static int _numPage;
        private static int _countRowOnPage;
        
        public static string UserLastPath
        {
            get => _userLastPath;
            set => _userLastPath = value;
        }
        public static List<SubElement> Content => _content;
        public static int CountRowOnPage => _countRowOnPage;

        public static int NumPage
        {
            get => _numPage;
            set => _numPage = value;
        }

        public UiClass()
        {
            //Console.CursorVisible = false;

            Console.SetWindowSize(120, 41);
            Console.SetBufferSize(120, 41);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Title = WindowName;

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
                _countRowOnPage = Convert.ToInt32(appSettings["_countRowOnPage"]);
                //JournalPath = appSettings["JournalPath"];
            }

            GetContent(_firstPath);

            PrintSectionContent();

            var processingUserCommand = new ProcessingUserCommand();
        }

        internal static void SetCursorToCommandPosition(string text)
        {
            for (int i = ColumnToCommand; i < 120; i++)
            {
                Console.SetCursorPosition(i, RowToCommand);
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, RowToCommand);
            Console.Write(text);
            Console.SetCursorPosition(ColumnToCommand, RowToCommand);
        }

        internal static void SetCursorToCommandPosition(string text, int columnToNewCommand)
        {
            for (int i = 0; i < 120; i++)
            {
                Console.SetCursorPosition(i, RowToCommand);
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, RowToCommand);
            Console.Write(text);
            Console.SetCursorPosition(columnToNewCommand, RowToCommand);
        }

        /// <summary>
        /// Получение содержимого папки
        /// </summary>
        /// <param name="pathToDirectory"> Путь к папке</param>
        public static void GetContent(string pathToDirectory)
        {
            var subElements = new SubElements(pathToDirectory);

            _content = new List<SubElement>();
            _content.Clear();

            _content = subElements.Contents;
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

        private static void PrintContentInfo(List<SubElement> subElements)
        {
            var rowToPrint = 2;
            var numPosition = 0;
            foreach (var subElement in subElements)
            {
                Console.SetCursorPosition(1, rowToPrint);
                Console.WriteLine((_numPage * _countRowOnPage) + 1 + numPosition++);

                Console.SetCursorPosition(6, rowToPrint);
                Console.WriteLine(subElement.NameElement);

                Console.SetCursorPosition(60, rowToPrint);
                Console.WriteLine(subElement.TypeElement);

                Console.SetCursorPosition(72, rowToPrint);
                Console.WriteLine(subElement.TypeAccessToElement ? "" : "ReadOnly");

                Console.SetCursorPosition(85, rowToPrint);
                Console.WriteLine(subElement.DateCreateElement);

                Console.SetCursorPosition(108, rowToPrint);
                Console.WriteLine(subElement.ElementSize == 0 ? "" : subElement.ElementSize.ToString());

                rowToPrint++;
            }
        }

        /// <summary>
        /// Функция для инициализации печати рамок таблицы
        /// </summary>
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
            Console.SetCursorPosition(1, rowToPrintHeader);
            Console.Write("№");
            PrintVerticalBorder(4);

            Console.SetCursorPosition(6, rowToPrintHeader);
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
            Console.Write("Размер (KБ)");

            Console.SetCursorPosition(0, RowToCommand);
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
                Console.Write('-');
                Console.SetCursorPosition(i, RowToCommand - 1);
                Console.Write('-');
            }

            Console.SetCursorPosition(58, rowsToTopMargin);
            Console.Write('+');
            Console.SetCursorPosition(58, RowToCommand - 1);
            Console.Write('+');

            Console.SetCursorPosition(70, rowsToTopMargin);
            Console.Write('+');
            Console.SetCursorPosition(70, RowToCommand - 1);
            Console.Write('+');

            Console.SetCursorPosition(84, rowsToTopMargin);
            Console.Write('+');
            Console.SetCursorPosition(84, RowToCommand - 1);
            Console.Write('+');

            Console.SetCursorPosition(105, rowsToTopMargin);
            Console.Write('+');
            Console.SetCursorPosition(105, RowToCommand - 1);
            Console.Write('+');
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
                Console.Write('|');
            }
        }

        /// <summary>
        /// Печать ограниченного кол-ва строк информации на странице
        /// </summary>
        public static void PrintSectionContent()
        {
            Console.Clear();

            PrintTable();

            GetElementsToPrint();

            if (_contentToPrint.Count > 0)
            {
                PrintContentInfo(_contentToPrint);
            }

            SetCursorToCommandPosition("Введите команду:");
        }

        /// <summary>
        /// Получение коллекции для вывода пользователю
        /// </summary>
        private static void GetElementsToPrint()
        {
            _contentToPrint = new List<SubElement>();
            _contentToPrint.Clear();

            var startIndex = _numPage * _countRowOnPage;
            var endIndex = startIndex + _countRowOnPage - 1;

            var realEndPosition = endIndex >= _content.Count ? _content.Count - 1 : endIndex;

            for (var i = startIndex; i <= realEndPosition; i++)
            {
                _contentToPrint.Add(_content[i]);
            }
        }

        internal static void PrintNegativeMessage(string message)
        {
            SetCursorToCommandPosition("Введите команду:");
            Console.Write(message);
            Console.ReadLine();
        }

        internal static string PrintMessageToUser(string message)
        {
            SetCursorToCommandPosition(message, message.Length+2);
            return Console.ReadLine();
        }
    }
}