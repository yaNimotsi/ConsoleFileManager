using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleFileManager
{
    class FoldersClasse
    {
        private List<FolderClass> _subFolders = new List<FolderClass>();

        public List<FolderClass> ListFolderClass
        {
            get { return _subFolders; }
        }

        public FoldersClasse(DirectoryInfo[] directoryInfos)
        {
            AddSubFolders(directoryInfos);
        }

        private void AddSubFolders(DirectoryInfo[] directoryInfos)
        {
            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                FolderClass folderClass = new FolderClass(directoryInfo);
                _subFolders.Add(folderClass);
            }
        }
    }
}
