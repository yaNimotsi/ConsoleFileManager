using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    internal static class OperationWithFile
    {
        
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

        /// <summary>
        /// Create file by name
        /// </summary>
        /// <param name="command">User command</param>
        public static void CreateFile(IReadOnlyList<string> command)
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

            if (File.Exists(fullPath))
            {
                PrintNegativeMessage("Папка уже существует или введено недопустимое имя папки");
                return;
            }

            try
            {
                File.Create(fullPath);
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

            UpdateConsoleWindow(UiClass.UserLastPath);
        }

        /// <summary>
        /// Copy file
        /// </summary>
        /// <param name="command">User command</param>
        public static void Copy(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentFilePath = command.TrimStart('c', 'o', 'p', 'y').Trim();

            currentFilePath = currentFilePath.Contains(Path.DirectorySeparatorChar) ? currentFilePath : Path.Combine(UiClass.UserLastPath, currentFilePath);

            if (IsFileExist(currentFilePath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите куда копировать:").Trim();

                newPath = newPath.Contains(Path.DirectorySeparatorChar) ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

                if (IsFileExist(newPath))
                {
                    PrintNegativeMessage("По указанному пути папка уже существует");
                    return;
                }

                File.Copy(currentFilePath, newPath, false);

                UpdateConsoleWindow(UiClass.UserLastPath);
            }
            else
            {
                PrintNegativeMessage(negativeMessage);
            }
        }

        /// <summary>
        /// Move file
        /// </summary>
        /// <param name="command">User command</param>
        public static void Move(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentFilePath = command.TrimStart('m', 'o', 'v', 'e').Trim();

            currentFilePath = currentFilePath.Contains(Path.DirectorySeparatorChar) ? currentFilePath : Path.Combine(UiClass.UserLastPath, currentFilePath);

            if (IsFileExist(currentFilePath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите куда переместить:").Trim();

                newPath = newPath.Contains(Path.DirectorySeparatorChar) ? newPath : Path.Combine(UiClass.UserLastPath, newPath);
                
                File.Move(currentFilePath, newPath, false);

                UpdateConsoleWindow(UiClass.UserLastPath);
            }
            else
            {
                PrintNegativeMessage(negativeMessage);
            }
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="command">User command</param>
        public static void Delete(IReadOnlyList<string> command)
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
                File.Delete(fullPath);
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

            UpdateConsoleWindow(UiClass.UserLastPath);
        }

        /// <summary>
        /// Rename file
        /// </summary>
        /// <param name="command">User command</param>
        public static void Rename(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentFilePath = command.TrimStart('r', 'e', 'n').Trim();

            currentFilePath = currentFilePath.Contains(Path.DirectorySeparatorChar) ? currentFilePath : Path.Combine(UiClass.UserLastPath, currentFilePath);

            if (IsFileExist(currentFilePath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите новое имя:").Trim();

                newPath = newPath.Contains(Path.DirectorySeparatorChar) ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

                File.Move(currentFilePath, newPath);

                UpdateConsoleWindow(UiClass.UserLastPath);
            }
            else
            {
                PrintNegativeMessage(negativeMessage);
            }
        }

        /// <summary>
        /// Check is file exist
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>True if exist otherwise false</returns>
        private static bool IsFileExist(string path)
        {
            return File.Exists(path);
        }
    }
}
