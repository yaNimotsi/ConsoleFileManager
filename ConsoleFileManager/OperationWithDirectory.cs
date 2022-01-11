using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    internal class OperationWithDirectory : IOperations
    {
        public void GoToDirectory(string userCommand)
        {
            if (string.IsNullOrWhiteSpace(userCommand))
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Недопустимая команда");
                return;
            }

            var newPath = userCommand.TrimStart('c', 'd').Trim();

            var result = newPath.Contains((char)92) ? GoToNewUserDirectory(newPath) : GoToSubDirectory(newPath);

            if (result) return;

            UiClass.SetCursorToCommandPosition("Введите команду:");
            Console.Write("Недопустимая команда");
            Console.ReadLine();
        }

        /// <summary>
        /// Переход в дирректорию по указанному пользователем пути
        /// </summary>
        /// <param name="newPath">Полный путь к папке</param>
        /// <returns></returns>
        private static bool GoToNewUserDirectory(string newPath)
        {
            if (!Directory.Exists(newPath)) return false;

            UpdateConsoleWindow(newPath);

            return true;
        }

        /// <summary>
        /// Переход к под папке текущей дирректории
        /// </summary>
        /// <param name="nameSubDirectory">Название под дирректории</param>
        /// <returns></returns>
        private static bool GoToSubDirectory(string nameSubDirectory)
        {
            var newPath = Path.Combine(UiClass.UserLastPath, nameSubDirectory);

            if (!Directory.Exists(newPath)) return false;

            UpdateConsoleWindow(newPath);

            return true;
        }

        /// <summary>
        /// Update data in console
        /// </summary>
        /// <param name="newPath"></param>
        private static void UpdateConsoleWindow(string newPath)
        {
            Console.Title = UiClass.WindowName + " Путь:" + newPath;
            UiClass.NumPage = 0;
            UiClass.UserLastPath = newPath;

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.PrintSectionContent();
        }

        public void CreateDirectory(IReadOnlyList<string> command)
        {
            var fullPath = "";

            for (var i = 1; i < command.Count; i++)
            {
                fullPath += command[i];
            }

            if (!fullPath.Contains(Path.DirectorySeparatorChar))
            {
                fullPath += UiClass.UserLastPath;
            }

            if (Directory.Exists(fullPath))
            {
                PrintNegativeMessage("Папка уже существует или введено недопустимое имя папки");
                return;
            }

            try
            {
                Directory.CreateDirectory(fullPath);
            }
            catch (PathTooLongException e)
            {
                PrintNegativeMessage(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                PrintNegativeMessage(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                PrintNegativeMessage(e.Message);
            }
            catch (IOException e)
            {
                PrintNegativeMessage(e.Message);
            }

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.NumPage = 0;
            UiClass.PrintSectionContent();
        }

        public void Copy(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentDirectoryPath = command.TrimStart('c', 'o', 'p', 'y').Trim();

            currentDirectoryPath = currentDirectoryPath.Contains(Path.DirectorySeparatorChar) ? currentDirectoryPath : Path.Combine(UiClass.UserLastPath, currentDirectoryPath);

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите куда копировать:").Trim();

                newPath = newPath.Contains(Path.DirectorySeparatorChar) ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

                if (IsDirectoryExist(newPath))
                {
                    PrintNegativeMessage("По указанному пути папка уже существует");
                    return;
                }

                Directory.CreateDirectory(newPath);
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(currentDirectoryPath, newPath);
            }
            else
            {
                PrintNegativeMessage(negativeMessage);
            }
        }

        public void Move(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentDirectoryPath = command.TrimStart('m', 'o', 'v', 'e').Trim();

            currentDirectoryPath = currentDirectoryPath.Contains(Path.DirectorySeparatorChar) ? currentDirectoryPath : Path.Combine(UiClass.UserLastPath, currentDirectoryPath);

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите куда переместить:").Trim();

                newPath = newPath.Contains(Path.DirectorySeparatorChar) ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

                Directory.CreateDirectory(newPath);
                Directory.Move(currentDirectoryPath, newPath);
            }
            else
            {
                PrintNegativeMessage(negativeMessage);
            }
        }
        
        public void Delete(IReadOnlyList<string> command)
        {
            var fullPath = "";

            for (var i = 1; i < command.Count; i++)
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
                PrintNegativeMessage(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                PrintNegativeMessage(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                PrintNegativeMessage(e.Message);
            }
            catch (IOException e)
            {
                PrintNegativeMessage(e.Message);
            }

            UiClass.GetContent(UiClass.UserLastPath);
            UiClass.NumPage = 0;
            UiClass.PrintSectionContent();
        }

        public void Rename(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentDirectoryPath = command.TrimStart('r', 'e', 'n').Trim();

            currentDirectoryPath = currentDirectoryPath.Contains(Path.DirectorySeparatorChar) ? currentDirectoryPath : Path.Combine(UiClass.UserLastPath, currentDirectoryPath);

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите новое имя:").Trim();

                newPath = newPath.Contains(Path.DirectorySeparatorChar) ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

                Directory.Move(currentDirectoryPath, newPath);

                GoToNewUserDirectory(UiClass.UserLastPath);
            }
            else
            {
                PrintNegativeMessage(negativeMessage);
            }
        }

        private static bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Вовод сообщения пользователю в случае недопустимой комманды
        /// </summary>
        /// <param name="message"> Сообщение для вывода</param>
        private static void PrintNegativeMessage(string message)
        {
            UiClass.SetCursorToCommandPosition("Введите команду:");
            Console.Write(message);
            Console.ReadLine();
        }
    }
}
