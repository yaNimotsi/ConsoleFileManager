using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleFileManager
{
    internal abstract class OperationWithDirectory : IDirectoryOperations
    {
        public void GoToDirectory(string newPath)
        {
            if (string.IsNullOrWhiteSpace(newPath))
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Недопустимая команда");
            }
            else
            {
                var finalPath = newPath.TrimStart('c', 'd').Trim();

                var result = newPath.Contains((char)92) ? GoToNewUserDirectory(newPath) : GoToSubDirectory(newPath);

                if (result) return;

                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Недопустимая команда");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Переход в дирректорию по указанному пользователем пути
        /// </summary>
        /// <param name="newPath">Полный путь к папке</param>
        /// <returns></returns>
        private static bool GoToNewUserDirectory(string newPath)
        {
            if (!Directory.Exists(newPath)) return false;

            Console.Title = UiClass.WindowName + " Путь:" + newPath;
            UiClass.NumPage = 0;
            UiClass.UserLastPath = newPath;

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.PrintSectionContent();

            return true;
        }

        /// <summary>
        /// Переход к под папке текущей дирректории
        /// </summary>
        /// <param name="nameSubDirectory">Название под дирректории</param>
        /// <returns></returns>
        private static bool GoToSubDirectory(string nameSubDirectory)
        {
            var fullPath = Path.Combine(UiClass.UserLastPath, nameSubDirectory);

            if (!Directory.Exists(fullPath)) return false;

            Console.Title = UiClass.WindowName + " Путь:" + fullPath;
            UiClass.NumPage = 0;
            UiClass.UserLastPath = fullPath;

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.PrintSectionContent();

            return true;
        }

        public void CreateDirectory(IReadOnlyList<string> newDirectoryName)
        {
            var fullPath = "";

            for (int i = 1; i < newDirectoryName.Count; i++)
            {
                fullPath += newDirectoryName[i];
            }

            if (!fullPath.Contains(Path.DirectorySeparatorChar))
            {
                fullPath += UiClass.UserLastPath;
            }

            if (Directory.Exists(fullPath))
            {
                UiClass.PrintNegativeMessage("Папка уже существует или введено недопустимое имя папки");
                return;
            }

            try
            {
                Directory.CreateDirectory(fullPath);
            }
            catch (PathTooLongException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }
            catch (IOException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.NumPage = 0;
            UiClass.PrintSectionContent();
        }

        public void Copy(string currentPath, string newPath)
        {
            throw new NotImplementedException();
        }

        public void Move(string currentPath, string newPath)
        {
            throw new NotImplementedException();
        }

        public void Delete(IReadOnlyList<string> command)
        {
            var fullPath = "";

            for (int i = 1; i < command.Count; i++)
            {
                fullPath += command[i];
            }

            if (!fullPath.Contains(Path.DirectorySeparatorChar))
            {
                fullPath += UiClass.UserLastPath;
            }

            try
            {
                Directory.Delete(fullPath);
            }
            catch (PathTooLongException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }
            catch (IOException e)
            {
                UiClass.PrintNegativeMessage(e.Message);
            }

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.NumPage = 0;
            UiClass.PrintSectionContent();
        }

        public void Rename(string currentPath, string newName)
        {
            throw new NotImplementedException();
        }
    }
}
