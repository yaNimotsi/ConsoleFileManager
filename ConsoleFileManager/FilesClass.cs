using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    class FilesClass
    {
        //private string _parentFolder;


        public List<FileClass> Files { get; set; } = new List<FileClass>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="folderPath"> Путь к папке для просомтра</param>
        /// <param name="pathToFiles"> Пути к файлам внутри папки</param>
        public FilesClass(string folderPath, IEnumerable<string> pathToFiles)
        {
            //_parentFolder = folderPath;
            AddFiles(pathToFiles);
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="fileInfos"> Массив содержащий информацию о файлах</param>
        public FilesClass(FileInfo[] fileInfos)
        {
            AddFiles(fileInfos);
        }

        /// <summary>
        /// Добавление файлов в список для текущей папки
        /// </summary>
        /// <param name="pathToFiles"></param>
        public void AddFiles(IEnumerable<string> pathToFiles)
        {
            foreach (var currentFile in pathToFiles)
            {
                if (!File.Exists(currentFile)) continue;

                var newFile = new FileClass(currentFile);
                Files.Add(newFile);
            }
        }

        /// <summary>
        /// Перегруженный метод для добавления файлов в список
        /// </summary>
        /// <param name="fileInfos"></param>
        public void AddFiles(FileInfo[] fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                var newFile = new FileClass(fileInfo);
                Files.Add(newFile);
            }
        }
    }
}
