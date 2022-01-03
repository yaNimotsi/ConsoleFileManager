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
                UiClass.SetCursorToCommandPosition();
                command = Console.ReadLine()?.ToLower();
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
                UiClass.SetCursorToCommandPosition();
                Console.Write("Недопустимая команда");
                Console.ReadLine();
                return;
            }

            switch (arrFromCommand[0])
            {
                case "вперед":
                    IsMaybeMore();
                    break;
                case "назад":
                    IsMaybeLess();
                    break;
                case "cd":
                    GoToNewDirectory(command);
                    break;
                case "copy":
                    break;
                case "mkdir":
                    MakeNewDirectory(arrFromCommand);
                    break;
                case "del":
                    DeleteDirectory(arrFromCommand);
                    break;
                case "ren":
                    break;
                case "move":
                    break;
                default:
                    UiClass.SetCursorToCommandPosition();
                    Console.Write("Недопустимая команда");
                    Console.ReadLine();
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
                UiClass.SetCursorToCommandPosition();
                Console.Write("Недопустимая команда");
            }
            else
            {
                var newPath = userCommand.TrimStart('c', 'd').Trim();

                var result = newPath.Contains((char)92) ? GoToNewUserDirectory(newPath) : GoToSubDirectory(newPath);

                if (result) return;

                UiClass.SetCursorToCommandPosition();
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
            var fullPath = UiClass.UserLastPath + @"\" + nameSubDirectory;

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
                UiClass.SetCursorToCommandPosition();
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
                UiClass.SetCursorToCommandPosition();
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

            if (!fullPath.Contains((char)92))
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

            if (!fullPath.Contains((char)92))
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
            UiClass.SetCursorToCommandPosition();
            Console.Write(message);
            Console.ReadLine();
        }
    }
}