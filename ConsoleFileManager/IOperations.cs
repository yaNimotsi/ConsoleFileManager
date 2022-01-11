using System.Collections.Generic;

namespace ConsoleFileManager
{
    interface IOperations
    {
        //void GoToDirectory(string newPath);
        //void CreateDirectory(IReadOnlyList<string> newDirectoryName);
        void Copy(string command);
        void Move(string command);
        void Delete(IReadOnlyList<string> command);
        void Rename(string command);

    }
}
