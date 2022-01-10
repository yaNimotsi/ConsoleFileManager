using System.Collections.Generic;

namespace ConsoleFileManager
{
    interface IDirectoryOperations
    {
        void GoToDirectory(string newPath);
        void CreateDirectory(IReadOnlyList<string> newDirectoryName);
        void Copy(string currentPath, string newPath);
        void Move(string currentPath, string newPath);
        void Delete(IReadOnlyList<string> command);
        void Rename(string currentPath, string newName);

    }
}
