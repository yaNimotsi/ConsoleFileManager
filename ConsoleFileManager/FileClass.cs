﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleFileManager
{
    class FileClass
    {
        private const string readType = "Чтение";
        private const string writeType = "Запись";

        private string _nameFile;
        private string _extension;
        private string _fileAccesType;
        private string _dateCreate;
        private int _fileSyze;


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

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="filePath"> Путь до нового экземпляра класса</param>
        public FileClass(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                _nameFile = fileInfo.Name;
                _extension = fileInfo.Extension;
                _fileAccesType = fileInfo.IsReadOnly.ToString();
                _fileSyze = (int)fileInfo.Length;
            }
        }

        public FileClass(FileInfo fileInfo)
        {
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
            if (File.Exists(newPath))
            {
                string userVal = "";

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
                catch { }
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

