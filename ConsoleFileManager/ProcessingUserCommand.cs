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
                    UiClass.PrintNegativeMessage("Команда не распознана");
                    break;
            }
        }

        /// <summary>
        /// Обработка команды перехода к папке
        /// </summary>
        /// <param name="userCommand"> Указанный пользователем путь</param>
        private static void GoToNewDirectory(string userCommand)
        {
            var operationWithDirectory = new OperationWithDirectory();
            operationWithDirectory.GoToDirectory(userCommand);
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

        /// <summary>
        /// Delete directory
        /// </summary>
        /// <param name="command">User command</param>
        private static void DeleteDirectory(IReadOnlyList<string> command)
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
        /// Copy directory with sub elements
        /// </summary>
        /// <param name="command">User command</param>
        private static void CopyDirectory(string command)
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

        /// <summary>
        /// Move directory and sub elements 
        /// </summary>
        /// <param name="command">User command</param>
        private static void MoveDirectory(string command)
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

        /// <summary>
        /// Change name to directory
        /// </summary>
        /// <param name="command">User command</param>
        private static void RenameDirectory(string command)
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
    }
}