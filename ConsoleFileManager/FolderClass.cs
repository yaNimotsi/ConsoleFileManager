using System;
using System.Globalization;
using System.IO;

namespace ConsoleFileManager
{
    class FolderClass
    {
        private readonly string _pathFolder;
        private FilesesClass _subFiles;
        //private DateTime _dateCreateFolder;
        private readonly string _dateCreateFolder;
        private readonly string _folderName;
        private readonly DirectoryInfo _directoryInfo;

        public string PathFolder => _pathFolder;

        /*public FilesesClass SubFileses
        {
            get { return _subFiles; }
            set { _subFiles = value; }
        }*/

        public string DateCreateFolder => _dateCreateFolder;

        public string FolderName => _folderName;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="pathToCurrentFolder"></param>
        public FolderClass(string pathToCurrentFolder)
        {
            if (!Directory.Exists(pathToCurrentFolder)) return;

            _pathFolder = pathToCurrentFolder;
            _dateCreateFolder = Directory.GetCreationTime(pathToCurrentFolder).ToString(CultureInfo.InvariantCulture);
        }

        public FolderClass(DirectoryInfo directoryInfo)
        {
            _pathFolder = directoryInfo.FullName;
            _dateCreateFolder = directoryInfo.CreationTime.ToShortDateString();
            _folderName = directoryInfo.Name;
            _directoryInfo = directoryInfo;
        }

        #region CopyFolder
        /// <summary>
        /// Запуск копирования папки в другую дирректорию
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPaht"></param>
        public void CopyFolder(string sourcePath, string targetPaht)
        {
            DirectoryInfo sourceInfo = new DirectoryInfo(sourcePath);
            DirectoryInfo targetInfo = new DirectoryInfo(targetPaht);

            CopyFolderAndAllInFolder(sourceInfo, targetInfo);
        }

        private static void CopyFolderAndAllInFolder(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                //Проверка на различие путей исходной и целевой директории
                if (source.FullName.ToLower() == target.FullName.ToLower())
                    return;

                //Проверка существования целевой директории
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }

                //Копирование файлов в текущей директории
                foreach (FileInfo fi in source.GetFiles())
                {
                    //Копирование файла. Если файл существует, то он будет перезаписан
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }


                //Копирование каждой поддиректоии и их файлов рекурсивным методом
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyFolderAndAllInFolder(diSourceSubDir, nextTargetSubDir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Процесс прерван: {0}", e);
            }

        }
        #endregion

        #region MoveFolder
        /// <summary>
        /// Копирование директории по указанному пути
        /// </summary>
        /// <param name="sourcePath">Текущее расположение папки</param>
        /// <param name="targetPath">Путь, куда будет произведено копирование</param>
        public void MoveFolder(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(sourcePath))
                return;

            Directory.Move(sourcePath, targetPath);
        }
        #endregion

        #region DeleteFolder
        /// <summary>
        /// Удаление дирректории.Если есть подкаталоги, то удаление происходит после подтвержения пользователя
        /// </summary>
        /// <param name="sourcePath"> Путь до папки, подлежащей удалению</param>
        public void DeleteFolder(string sourcePath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (_subFiles.Files.Count == 0)
                    Directory.Delete(sourcePath);
                else
                {
                    Console.WriteLine("Папка Не пустая. Вы хотите удалить папку и все ее содержимое?");
                    Console.WriteLine("Введите \"Y\" если хотите удалить, иначе введите \"N\"");

                    var userVal = Console.ReadLine();

                    while (userVal != "Y" || userVal != "N")
                    {
                        Console.WriteLine("Введено недопустимое значение. Повторите попытку");
                        Console.WriteLine("Папка Не пустая. Вы хотите удалить папку и все ее содержимое?");
                        Console.WriteLine("Введите \"Y\" если хотите удалить, иначе введите \"N\"");

                        userVal = Console.ReadLine();
                    }

                if (userVal == "Y")
                    DeleteFolder(sourcePath, true);
                else
                    Console.WriteLine("Удаление отменено");
            }
            else
            {
                Console.WriteLine("Папка, по указанному пути, не найдена");
            }
        }

        private static void DeleteFolder(string currentFolderPath, bool flag)
        {
            try
            {
                Directory.Delete(currentFolderPath, flag);
            }
            catch (Exception e) { Console.WriteLine($"Произошла ошибка - {e}"); }
        }
        #endregion
    }
}
