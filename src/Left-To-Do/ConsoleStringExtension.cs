using System;
using System.Collections.Generic;
using ToDoTask;

namespace LeftToDo
{
    static class ConsoleStringExtension
    {
        /* ------------------------------ Console Write ----------------------------- */
        // Skriver ut text i konsolfönstret med aningen Console.WriteLine() eller Console.Write()
        public static void WriteLineToConsole(this string str)
        {
            Console.WriteLine(str);
            Console.WriteLine();
        }

        /* --------------------------- Clear Console Write -------------------------- */
        // Skriver ut text i konsolfönstret med aningen Console.WriteLine() eller Console.Write(),
        // med Console.Clear();
        public static void ClearWriteLineToConsole(this string str)
        {
            Console.Clear();
            Console.WriteLine(str);
            Console.WriteLine();
        }
        public static void ClearWriteLineToConsole(this string[] stringArray)
        {
            Console.Clear();
            foreach (string str in stringArray)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine();
        }

        /* ------------------------------ Console Read ------------------------------ */
        // Skriver ut en text och returnerar användarens input
        public static string ForReadLine(this string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }
        public static char ForReadKey(this string text)
        {
            Console.Write(text);
            return Console.ReadKey().KeyChar;
        }

        /* -------------------------------- Messages -------------------------------- */
        // Skriver ut meddelanden med visuella variationer
        public static void WriteLineAsError(this string text)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.WriteLine();
            Console.ResetColor();
        }

        /* -------------------------- Task List Extensions -------------------------- */
        // Skriver ut TimelessTask
        public static void WriteLineToConsole(this List<TimelessTask> taskList, bool includeIndex, int pad)
        {
            string indexStr = "";
            for (int i = 0; i < taskList.Count; i++)
            {
                if (includeIndex)
                {
                    indexStr = $"[{i + 1}]";
                }

                if (taskList[i].IsDone)
                {
                    Console.WriteLine($"{indexStr} \u2611 {taskList[i].Note}".PadLeft(taskList[i].Note.Length + indexStr.Length + pad)); // Färdig
                }
                else
                {
                    Console.WriteLine($"{indexStr} \u2610 {taskList[i].Note}".PadLeft(taskList[i].Note.Length + indexStr.Length + pad)); // Ofärdig
                }
            }

            Console.WriteLine();
        }
        public static void WriteLineToConsole(this TimelessTask task)
        {
            if (task.IsDone)
            {
                Console.WriteLine($"\u2611 {task.Note}"); // Färdig
            }
            else
            {
                Console.WriteLine($"\u2610 {task.Note}"); // Ofärdig
            }
            Console.WriteLine();
        }

        // Skriver ut Deadline
        public static void WriteLineToConsole(this DeadlineTask task)
        {
            if (task.IsDone)
            {
                Console.WriteLine($"\u2611 {task.Note}");
                Console.WriteLine($"{task.Deadline.GetDaysTillDeadline()} dagar kvar!");
            }
            else
            {
                Console.WriteLine($"\u2610 {task.Note}"); // Ofärdig
                Console.WriteLine($"{task.Deadline.GetDaysTillDeadline()} dagar kvar!");
            }
            Console.WriteLine();
        }

        // Skriver ut Checklist
        public static void WriteLineToConsole(this ChecklistTask task, bool includeIndex, int pad)
        {
            if (task.IsDone)
            {
                Console.WriteLine($"\u2611 {task.Note}"); // Färdig
            }
            else
            {
                Console.WriteLine($"\u2610 {task.Note}"); // Ofärdig
            }

            task.TimelessTaskList.WriteLineToConsole(includeIndex, pad);

            Console.WriteLine();
        }

        public static void WriteLineToConsole(this List<Task> taskList, bool includeIndex)
        {
            string indexStr = "";
            for (int i = 0; i < taskList.Count; i++)
            {
                if (includeIndex)
                {
                    indexStr = $"[{i + 1}] ";
                    Console.Write(indexStr);
                }

                if (taskList[i] is TimelessTask)
                {
                    TimelessTask tempTimelessTask = (TimelessTask)taskList[i];
                    tempTimelessTask.WriteLineToConsole();
                }
                else if (taskList[i] is DeadlineTask)
                {
                    DeadlineTask tempDeadlineTask = (DeadlineTask)taskList[i];
                    tempDeadlineTask.WriteLineToConsole();
                }
                else if (taskList[i] is ChecklistTask)
                {
                    ChecklistTask tempDeadlineTask = (ChecklistTask)taskList[i];
                    tempDeadlineTask.WriteLineToConsole(false, indexStr.Length + 4);
                }
            }
        }
    }
}