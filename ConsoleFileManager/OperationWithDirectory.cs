using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    internal class OperationWithDirectory : IOperations
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

                var result = finalPath.Contains(Path.DirectorySeparatorChar) ? GoToNewUserDirectory(finalPath) : GoToSubDirectory(finalPath);

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

            for (var i = 1; i < newDirectoryName.Count; i++)
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

        public void Copy(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentDirectoryPath = command.TrimStart('c', 'o', 'p', 'y').Trim();

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите куда копировать:").Trim();

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

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите куда переместить:").Trim();

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

        public void Rename(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentDirectoryPath = command.TrimStart('r', 'e', 'n').Trim();

            currentDirectoryPath = currentDirectoryPath.Contains(Path.DirectorySeparatorChar) ? currentDirectoryPath : Path.Combine(UiClass.UserLastPath, currentDirectoryPath);

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите новое имя:").Trim();

                newPath = newPath.Contains(':') ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

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
