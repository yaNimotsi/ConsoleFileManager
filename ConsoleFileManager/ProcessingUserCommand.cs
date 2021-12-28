using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFileManager
{
    class ProcessingUserCommand
    {
        public ProcessingUserCommand()
        {
            GetUserCommand();
        }

        private void GetUserCommand()
        {
            var command = "";
            while (!string.Equals(command.ToLower(), "Выход".ToLower()))
            {
                command = Console.ReadLine();
                UiClass.SetCursorToCommandPosition();
            }
        }
    }
}
