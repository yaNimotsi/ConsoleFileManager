using System.Collections.Generic;
using System.Linq;

namespace ConsoleFileManager
{
    internal class SubElements
    {
        public List<SubElement> Contents { get; }

        private SubElements()
        {

        }

        public SubElements(string pathToCurrentFolder)
        {
            //Получаем содержимое папки
            var subDirectory = FolderContents.GetSubFolders(pathToCurrentFolder).ToList();
            var subFiles = FolderContents.GetSubFiles(pathToCurrentFolder).ToList();

            //Если коллекции пустые, то выходим
            if (subDirectory.Count <= 0 && subFiles.Count <= 0) return;

            Contents = new List<SubElement>();

            foreach (var directoryInfo in subDirectory)
            {
                Contents.Add(new SubElement(directoryInfo));
            }

            foreach (var fileInfo in subFiles)
            {
                Contents.Add(new SubElement(fileInfo));
            }
        }
    }
}
