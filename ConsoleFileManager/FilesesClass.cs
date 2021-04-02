using System;
using System.Collections.Generic;
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

        public FilesesClass(FolderClass folder)
        {
            _parentFolder = folder.PathFolder;
        }
    }
}
