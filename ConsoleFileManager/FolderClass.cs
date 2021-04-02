using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    class FolderClass
    {
        private string _pathFolder;
        private FilesesClass _subFiles;

        public string PathFolder
        {
            get { return _pathFolder; }
            set { _pathFolder = value; }
        }

        public FilesesClass SubFileses
        {
            get { return _subFiles; }
            set { _subFiles = value; }
        }


        public void CopyFolder()
        {

        }

        public void MoveFolder(string currentFolderPath)
        {

        }

        #region DeleteFolder
        /// <summary>
        /// Удаление дирректории.Если есть подкаталоги, то удаление происходит после подтвержения пользователя
        /// </summary>
        /// <param name="currentFolderPath"> Путь до папки, подлежащей удалению</param>
        public void DeleteFolder(string currentFolderPath)
        {
            if (Directory.Exists(currentFolderPath))
            {
                if (_subFiles.Files.Count == 0)
                    Directory.Delete(currentFolderPath);
                else
                    Console.WriteLine("Папка Не пустая. Вы хотите удалить папку и все ее содержимое?");
                    Console.WriteLine("Введите \"Y\" если хотите удалить, иначе введите \"N\"");

                    string userVal = Console.ReadLine();

                    while (userVal != "Y" || userVal != "N")
                    {
                        Console.WriteLine("Введено недопустимое значение. Повторите попытку");
                        Console.WriteLine("Папка Не пустая. Вы хотите удалить папку и все ее содержимое?");
                        Console.WriteLine("Введите \"Y\" если хотите удалить, иначе введите \"N\"");

                        userVal = Console.ReadLine();
                    }

                if (userVal == "Y")
                    DeleteFolder(currentFolderPath, true);
                else
                    Console.WriteLine("Удаление отменено");
            }
            else
                Console.WriteLine("Папка, по указанному пути, не найдена");
        }

        public void DeleteFolder(string currentFolderPath, bool flag)
        {
            try
            {
                Directory.Delete(currentFolderPath, flag);
            }
            catch (Exception e) { Console.WriteLine($"Произошла ошибка - {e}"); };
            
        }
        #endregion
    }
}
