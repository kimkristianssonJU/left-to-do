using System;
using System.Collections.Generic;
using ToDoTask;

namespace LeftToDo
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            App.UIGotoMainMenu(new ToDo());
        }
    }
}
