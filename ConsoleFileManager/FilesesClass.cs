using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleFileManager
{
    class FilesesClass
    {
        private string _parentFolder;
        private List<FileClass> _files;

        public string ParentFolder
        {
            get { return _parentFolder; }
            set { _parentFolder = value; }
        }

        public List<FileClass> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        List<FileClass> files = new List<FileClass>();

        public FilesesClass(string pathToFolder, IEnumerable<string> pathToFiles)
        {
            _parentFolder = pathToFolder;
            AddFileses(pathToFiles);
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
    }
}
