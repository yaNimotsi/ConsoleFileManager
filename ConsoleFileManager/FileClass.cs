using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleFileManager
{
    class FileClass
    {
        private readonly string _nameFile;
        private readonly string _extension;
        private readonly string _fileAccesType;
        private readonly string _dateCreate;

        private readonly int _fileSyze;

        private readonly FileInfo _fileInfo;


        //Имя файла
        public string NameFile
        {
            get { return _nameFile; }
        }

        //Расширение файла
        public string FileExtension
        {
            get { return _extension; }
        }


        //Тип доступа к файлу (чтение, запись...)
        public string FileAccesType
        {
            get { return _fileAccesType; }
        }

        public string DateCreate
        {
            get { return _dateCreate; }
        }


        //Размер файла
        public int FileSyze
        {
            get { return _fileSyze; }
        }

        public FileInfo CurrentFileInfo
        {
            get { return _fileInfo; }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="filePath"> Путь до нового экземпляра класса</param>
        public FileClass(string filePath)
        {
            if (File.Exists(filePath))
            {
                _fileInfo = new FileInfo(filePath);

                _nameFile = _fileInfo.Name;
                _extension = _fileInfo.Extension;
                _fileAccesType = _fileInfo.IsReadOnly.ToString();
                _fileSyze = (int)_fileInfo.Length;
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="fileInfo"> Провайдер содержащий информацию о файле и методы работы с ним</param>
        public FileClass(FileInfo fileInfo)
        {
            this._fileInfo = fileInfo;

            _nameFile = fileInfo.Name;
            _extension = fileInfo.Extension;
            _fileAccesType = fileInfo.IsReadOnly ? TypeAcces.Read.ToString() : TypeAcces.Write.ToString();
            _dateCreate = fileInfo.CreationTime.ToShortDateString();
            _fileSyze = (int)fileInfo.Length;
        }
        #region Method copyFile
        /// <summary>
        /// Метод копирования файла
        /// </summary>
        /// <param name="currentPath">Путь к исходному файлу</param>
        /// <param name="newPath">Путь, куда файл будет скопирован</param>
        public void CopyFile(string currentPath, string newPath)
        {
            string userVal;
            if (File.Exists(newPath))
            {
                Console.WriteLine("Файл, по новому пути, уже существует. Перезаписать его?");
                Console.WriteLine("Введите \"Y\" если хотите перезаписать файл, иначе введите \"N\"");

                userVal = Console.ReadLine();

                while (userVal != "Y" || userVal != "N")
                {
                    Console.WriteLine("Введено недопустимое значение. Повторите попытку");
                    Console.WriteLine("Файл, по новому пути, уже существует. Перезаписать его?");
                    Console.WriteLine("Введите \"Y\" если хотите перезаписать файл, иначе введите \"N\"");

                    userVal = Console.ReadLine();
                }

                if (userVal == "Y")
                    CopyFile(currentPath, newPath, true);
                else
                    Console.WriteLine("Копирование отменено");

            }
            else
                File.Copy(currentPath, newPath);
        }

        /// <summary>
        /// Метод копирования файла, на случай, если файл уже существует
        /// </summary>
        /// <param name="pathToFile">Путь к исходному файлу</param>
        /// <param name="newPathFile">Путь, куда файл будет скопирован</param>
        /// <param name="flag"> Флаг. Если true, то файл перезапишется, если false, то перезаписи не будет</param>
        public void CopyFile(string pathToFile, string newPathFile, bool flag)
        {
            File.Copy(pathToFile, newPathFile, flag);
        }
        #endregion

        #region MoveFile    
        public void MoveFile(string currentPath, string newPath)
        {
            if (File.Exists(currentPath))
            {
                if (!File.Exists(newPath))
                    File.Move(currentPath, newPath);
                else
                {
                    Console.WriteLine("Файл, по новому пути, уже существует. Перезаписать его?");
                    Console.WriteLine("Введите \"Y\" если хотите перезаписать файл, иначе введите \"N\"");

                    string userVal = Console.ReadLine();

                    while (userVal != "Y" || userVal != "N")
                    {
                        Console.WriteLine("Введено недопустимое значение. Повторите попытку");
                        Console.WriteLine("Файл, по новому пути, уже существует. Перезаписать его?");
                        Console.WriteLine("Введите \"Y\" если хотите перезаписать файл, иначе введите \"N\"");

                        userVal = Console.ReadLine();
                    }

                    if (userVal == "Y")
                        CopyFile(currentPath, newPath, true);
                    else
                        Console.WriteLine("Перемещение отменено");
                }
            }
            else
                Console.WriteLine("Исходный файл не найден. Проверьте корректность пути");
        }
        #endregion

        #region Delete
        static public void Delete(string currentPath)
        {
            if (File.Exists(currentPath))
                try
                {
                    File.Delete(currentPath);
                }
                catch
                {
                    // ignored
                }
            else
                Console.WriteLine("Файл, по указанному пути, не найден");
        }
        #endregion

        /// <summary>
        ///Перечисление типов доступа к файлу. 
        /// </summary>
        public enum TypeAcces
        {
            Read,
            Write,
            ReadAndWrite
        }
    }
}

