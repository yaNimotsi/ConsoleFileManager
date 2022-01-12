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

        // ReSharper disable once UnusedMember.Local
        private SubElement()
        {

        }

        public SubElement(FileInfo fileInfo)
        {
            NameElement = CheckLengthSize(fileInfo.Name , 47);
            TypeElement = CheckLengthSize(fileInfo.Extension , 6);
            TypeAccessToElement = fileInfo.IsReadOnly;
            DateCreateElement = fileInfo.CreationTime;
            ElementSize = ConvertBiteToKiloByte(fileInfo.Length);
            PathToElement = fileInfo.FullName;
        }

        public SubElement(DirectoryInfo directoryInfo)
        {
            NameElement = CheckLengthSize(directoryInfo.Name, 47);
            TypeElement = "Folder";
            TypeAccessToElement = GetDirectoryAccessType(directoryInfo.Attributes);
            DateCreateElement = directoryInfo.CreationTime;
            PathToElement = directoryInfo.FullName;
        }

        /// <summary>
        /// Если длина имени выше указанной, то обрезаем
        /// </summary>
        /// <param name="name">Elemet name</param>
        /// <param name="maxLength">Max acceptable lenght name</param>
        /// <returns></returns>
        private static string CheckLengthSize(string name , int maxLength)
        {
            if (name.Length > maxLength)
            {
                name = name.Remove(maxLength) + "...";
            }
            return name;
        }

        /// <summary>
        /// Получение типа доступа к папке
        /// </summary>
        /// <param name="attributes">Folder attributes</param>
        /// <returns>Folder acces type</returns>
        private static bool GetDirectoryAccessType(FileAttributes attributes)
        {
            return (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        /// <summary>
        /// Преобразование размера документа из bite в killoByte
        /// </summary>
        /// <param name="elementSizeInBite">File size in bytes</param>
        /// <returns>File size in Kb</returns>
        private static long ConvertBiteToKiloByte(long elementSizeInBite)
        {
            if (elementSizeInBite == 0) return 0;

            return elementSizeInBite / (8 * 1024);
        }
    }
}
