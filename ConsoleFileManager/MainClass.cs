using System;
using System.IO;

namespace ConsoleFileManager
{
    class MainClass
    {
        static void Main(string[] args)
        {
            UIClass newUI = new UIClass();

            Console.Write("Test print messege in diferent place 1");
            Console.SetCursorPosition(20, 3);
            Console.Write("Test print messege in diferent place 2");
            Console.SetCursorPosition(85, 3);
            Console.Write("Test print messege in diferent place 3");
        }
    }
}
