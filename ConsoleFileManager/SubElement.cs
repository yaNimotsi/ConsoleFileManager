using System;
using System.IO;

namespace ConsoleFileManager
{
    internal class SubElement
    {
        public string NameElement { get;}
        public string TypeElement { get;}
        public bool TypeAccessToElement { get;}
        public DateTime DateCreateElement { get;}
        public long ElementSize { get;}
        public string PathToElement { get;}

        private SubElement()
        {

        }

        public SubElement(FileInfo fileInfo)
        {
            NameElement = CheckLengthSize(fileInfo.Name , 50);
            TypeElement = CheckLengthSize(fileInfo.Extension , 6);
            TypeAccessToElement = fileInfo.IsReadOnly;
            DateCreateElement = fileInfo.CreationTime;
            ElementSize = fileInfo.Length;
            PathToElement = fileInfo.FullName;
        }
        public SubElement(DirectoryInfo directoryInfo)
        {
            NameElement = CheckLengthSize(directoryInfo.Name, 50);
            TypeElement = "Folder";
            TypeAccessToElement = GetFolderAccessType(directoryInfo.Attributes);
            DateCreateElement = directoryInfo.CreationTime;
            PathToElement = directoryInfo.FullName;
        }

        private static string CheckLengthSize(string name , int maxLength)
        {
            if (name.Length > maxLength)
            {
                name = name.Remove(maxLength) + "...";
            }
            return name;
        }

        private static bool GetFolderAccessType(FileAttributes attributes)
        {
            return (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }
    }
}
