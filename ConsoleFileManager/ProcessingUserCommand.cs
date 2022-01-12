using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

                if (!string.IsNullOrWhiteSpace(command))
                {
                    DetectCommand(command);
                }
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
                    MakeCopy(command);
                    break;
                case "mkdir":
                    MakeNewDirectory(command, arrFromCommand);
                    break;
                case "del":
                    DeleteDirectory(command, arrFromCommand);
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
            OperationWithDirectory.GoToDirectory(userCommand);
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
        /// <param name="arrFromCommand"></param>
        private static void MakeNewDirectory(string command, IReadOnlyList<string> arrFromCommand)
        {
            if (GetTypeByPath(command) == 0)
            {
                OperationWithDirectory.CreateDirectory(arrFromCommand);
            }
            else
            {
                OperationWithFile.CreateFile(arrFromCommand);
            }
        }

        /// <summary>
        /// Delete directory
        /// </summary>
        /// <param name="command">User command</param>
        /// <param name="arrFromCommand"></param>
        private static void DeleteDirectory(string command, IReadOnlyList<string> arrFromCommand)
        {
            if (GetTypeByPath(command) == 0)
            {
                OperationWithDirectory.Delete(arrFromCommand);
            }
            else
            {
                OperationWithFile.Delete(arrFromCommand);
            }
        }

        /// <summary>
        /// Copy directory with sub elements
        /// </summary>
        /// <param name="command">User command</param>
        private static void MakeCopy(string command)
        {
            if (GetTypeByPath(command) == 0)
            {
                OperationWithDirectory.Copy(command);
            }
            else
            {
                OperationWithFile.Copy(command);
            }
        }
        
        /// <summary>
        /// Move directory and sub elements 
        /// </summary>
        /// <param name="command">User command</param>
        private static void MoveDirectory(string command)
        {
            if (GetTypeByPath(command) == 0)
            {
                OperationWithDirectory.Move(command);
            }
            else
            {
                OperationWithFile.Move(command);
            }
        }

        /// <summary>
        /// Change name to directory
        /// </summary>
        /// <param name="command">User command</param>
        private static void RenameDirectory(string command)
        {
            if (GetTypeByPath(command) == 0)
            {
                OperationWithDirectory.Rename(command);
            }
            else
            {
                OperationWithFile.Rename(command);
            }
        }

        /// <summary>
        /// Get type by path
        /// </summary>
        /// <param name="path"> Path to object</param>
        /// <returns> 0 if it Directory and 1 if it File</returns>
        private static byte GetTypeByPath(string path)
        {
            if (!UiClass.Content.Any(element => string.Equals(element.PathToElement, path))) return 0;
            
            var fileAttribute = File.GetAttributes(path);

            return fileAttribute == FileAttributes.Directory ? (byte)0 : (byte)1;

        }
    }
}