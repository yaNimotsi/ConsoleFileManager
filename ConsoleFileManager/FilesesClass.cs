using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleFileManager
{
    class FilesesClass
    {
        //private string _parentFolder;
        private List<FileClass> _files = new List<FileClass>();

        /*public string ParentFolder
        {
            get { return _parentFolder; }
            set { _parentFolder = value; }
        }*/

        public List<FileClass> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="pathToFolder"> Путь к папке для просомтра</param>
        /// <param name="pathToFiles"> Пути к файлам внутри папки</param>
        public FilesesClass(string pathToFolder, IEnumerable<string> pathToFiles)
        {
            //_parentFolder = pathToFolder;
            AddFileses(pathToFiles);
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="fileInfos"> Массив содержащий информацию о файлах</param>
        public FilesesClass(FileInfo[] fileInfos)
        {
            AddFileses(fileInfos);
        }

        /// <summary>
        /// Добавление файлов в список для текущей папки
        /// </summary>
        /// <param name="pathToFiles"></param>
        public void AddFileses(IEnumerable<string> pathToFiles)
        {
            foreach (string currentFile in pathToFiles)
            {
                if (File.Exists(currentFile))
                {
                    FileClass newFile = new FileClass(currentFile);
                    _files.Add(newFile);
                }
            }
        }

        /// <summary>
        /// Перегруженный метод для добавления файлов в список
        /// </summary>
        /// <param name="fileInfos"></param>
        public void AddFileses(FileInfo[] fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                FileClass newFile = new FileClass(fileInfo);
                _files.Add(newFile);
            }
        }
    }
}
