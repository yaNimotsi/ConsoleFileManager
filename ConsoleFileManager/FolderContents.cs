using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    public static class FolderContents
    {
        private static readonly List<FileInfo> SubFilesInfo = new List<FileInfo>();
        private static readonly List<DirectoryInfo> SubDirectoriesInfo = new List<DirectoryInfo>();

        /// <summary>
        /// Get sub folders
        /// </summary>
        public static IEnumerable<DirectoryInfo> GetSubFolders(string pathToMainMainFolder)
        {
            if (string.IsNullOrWhiteSpace(pathToMainMainFolder))
            {
                return null;
            }

            if (File.Exists(pathToMainMainFolder)) return null;
            
            var directories = Directory.EnumerateDirectories(pathToMainMainFolder);

            SubDirectoriesInfo.Clear();

            foreach (var pathToDirectory in directories)
            {
                SubDirectoriesInfo.Add(new DirectoryInfo(pathToDirectory));
            }

            return SubDirectoriesInfo;
        }

        /// <summary>
        /// Get sub files
        /// </summary>
        public static IEnumerable<FileInfo> GetSubFiles(string pathToMainMainFolder)
        {
            if (string.IsNullOrWhiteSpace(pathToMainMainFolder))
            {
                return null;
            }

            if (File.Exists(pathToMainMainFolder)) return null;

            var files = Directory.EnumerateFiles(pathToMainMainFolder);

            SubFilesInfo.Clear();

            foreach (var pathToFile in files)
            {
                SubFilesInfo.Add(new FileInfo(pathToFile));
            }
            return SubFilesInfo;
        }
    }
}
