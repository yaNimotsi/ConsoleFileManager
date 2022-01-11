using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleFileManager
{
    internal class ProcessingUserCommand
    {
        public ProcessingUserCommand()
        {
            GetUserCommand();
        }

        /// <summary>
        /// Получение ввденной пользователем команды
        /// </summary>
        private static void GetUserCommand()
        {
            var command = "";
            while (command != null && !string.Equals(command.ToLower(), "Выход".ToLower()))
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                command = Console.ReadLine()?.ToLower().Trim();
                DetectCommand(command);
            }
        }

        /// <summary>
        /// Обработка введенной пользователем команды
        /// </summary>
        /// <param name="command">Введенная пользователем команда</param>
        private static void DetectCommand(string command)
        {
            var arrFromCommand = command.Split(" ");

            if (arrFromCommand.Length <= 0)
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Недопустимая команда");
                Console.ReadLine();
                return;
            }

            switch (arrFromCommand[0])
            {
                case "next":
                    IsMaybeMore();
                    break;
                case "back":
                    IsMaybeLess();
                    break;
                case "cd":
                    GoToNewDirectory(command);
                    break;
                case "copy":
                    CopyDirectory(command);
                    break;
                case "mkdir":
                    MakeNewDirectory(arrFromCommand);
                    break;
                case "del":
                    DeleteDirectory(arrFromCommand);
                    break;
                case "ren":
                    RenameDirectory(command);
                    break;
                case "move":
                    MoveDirectory(command);
                    break;
                default:
                    UiClass.PrintNegativeMessage("Недопустимая команда");
                    break;
            }
        }

        /// <summary>
        /// Обработка команды перехода к папке
        /// </summary>
        /// <param name="userCommand"> Указанный пользователем путь</param>
        private static void GoToNewDirectory(string userCommand)
        {
            if (string.IsNullOrWhiteSpace(userCommand))
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Недопустимая команда");
            }
            else
            {
                var newPath = userCommand.TrimStart('c', 'd').Trim();

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

        /// <summary>
        /// Проверка и при возможности показ предыдущей страницы с содержимым
        /// </summary>
        private static void IsMaybeLess()
        {
            if (UiClass.NumPage - 1 < 0)
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Куда уж меньше, завязывай ;)");
                Console.ReadLine();
            }
            else
            {
                UiClass.NumPage--;
                UiClass.PrintSectionContent();
            }
        }

        /// <summary>
        /// Проверка и при возможности показ следующей страницы с содержимым
        /// </summary>
        private static void IsMaybeMore()
        {
            if (UiClass.NumPage + 1 >= UiClass.Content.Count / UiClass.CountRowOnPage + (UiClass.Content.Count % UiClass.CountRowOnPage > 0 ? 1 : 0))
            {
                UiClass.SetCursorToCommandPosition("Введите команду:");
                Console.Write("Куда уж дальше, там бесконечность :Р");
                Console.ReadLine();
            }
            else
            {
                UiClass.NumPage++;
                UiClass.PrintSectionContent();
            }
        }

        /// <summary>
        /// Создание новой папки по указанному пути
        /// </summary>
        /// <param name="command"></param>
        private static void MakeNewDirectory(IReadOnlyList<string> command)
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

        private static void DeleteDirectory(IReadOnlyList<string> command)
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

        private static void CopyDirectory(string command)
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

        /*private static void CopyContents(string sourcePath, string targetPath)
        {
            if (!IsDirectoryExist(sourcePath) || !IsDirectoryExist(targetPath)) return;
            
            var filePaths = Directory.GetFiles(sourcePath);

            foreach (var s in filePaths)
            {
                var fileName = Path.GetFileName(s);
                var pathToFile = Path.Combine(targetPath, fileName);
                File.Copy(s, pathToFile, true);
            }

        }

        private static void CopyDirectories(string sourcePath, string targetPath)
        {
            if (!IsDirectoryExist(sourcePath) || !IsDirectoryExist(targetPath)) return;

            var directories = Directory.EnumerateDirectories(sourcePath);

            var dirs = new Stack<string>();

            dirs.Push(sourcePath);

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();

                if (!IsDirectoryExist(currentDir))
                {
                    Directory.CreateDirectory(currentDir);
                }

                string[] subDirs;

                try
                {
                    subDirs = Directory.(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string str in subDirs)
                {
                    dirs.Push(str);
                }
                    
            }
        }*/

        private static void MoveDirectory(string command)
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

        private static void RenameDirectory(string command)
        {
            const string negativeMessage = "Указанный путь не найден!";
            var currentDirectoryPath = command.TrimStart('r', 'e', 'n').Trim();

            currentDirectoryPath = currentDirectoryPath.Contains(':') ? currentDirectoryPath : Path.Combine(UiClass.UserLastPath, currentDirectoryPath);

            if (IsDirectoryExist(currentDirectoryPath))
            {
                var newPath = UiClass.PrintMessageToUser("Укажите новое имя:").Trim();

                newPath = newPath.Contains(':') ? newPath : Path.Combine(UiClass.UserLastPath, newPath);

                //Directory.CreateDirectory(newPath);
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
    }
}