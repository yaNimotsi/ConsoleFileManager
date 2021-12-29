using System;
using System.IO;

namespace ConsoleFileManager
{
    internal class ProcessingUserCommand
    {
        public ProcessingUserCommand()
        {
            GetUserCommand();
        }

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
                    break;
                case "rmdir":
                    break;
                case "del":
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

        private static void GoToNewDirectory(string userCommand)
        {
            if (string.IsNullOrWhiteSpace(userCommand))
            {
                UiClass.SetCursorToCommandPosition();
                Console.Write("Недопустимая команда");
            }
            else
            {
                var newPath = userCommand.TrimStart(new char[] { 'c', 'd' }).Trim();

                if (!Directory.Exists(newPath))
                {
                    UiClass.SetCursorToCommandPosition();
                    Console.Write("Недопустимая команда");
                }
                else
                {
                    Console.Title = UiClass.WindowName + " Путь:" + newPath;
                    UiClass.NumPage = 0;
                    UiClass.UserLastPath = newPath;

                    UiClass.GetContent(UiClass.UserLastPath);
                    UiClass.PrintSectionContent();
                }
            }
        }

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
    }
}