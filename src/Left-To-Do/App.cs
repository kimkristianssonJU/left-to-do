using System;
using System.Collections.Generic;
using System.Globalization;
using ToDoTask;

namespace LeftToDo
{
    static class App
    {
        static string _escapeWord = "!klar";
        static string _cancelWord = "!avbryt";
        static string _noteLabel = $"Skriv in din uppgift här eller skriv {_cancelWord} för att avbryta: ";
        static string _checklistLabel = $"Ge en beskrivning av checklistan eller {_cancelWord} för att avbryta: ";
        static readonly string[] _mainMenuOptions =
        {
            "LEFT TO DO",
            "Gör ditt menyval genom att trycka på en siffra",
            "",
            "[1] Lägg till en ny uppgift",
            "[2] Lista alla dagens uppgifter",
            "[3] Lista alla akriverade uppgifter",
            "[4] Arkivera alla avklarade uppgifter",
            "[Q] Avsluta programmet"
        };
        static string[] _addTaskMenuOptions =
        {
            "NY UPPGIFT",
            "Vilken typ av uppgift vill du skapa?",
            "",
            "[1] Tidlös uppgift",
            "[2] Uppgift med deadline",
            "[3] Uppgift med checklista",
            "[Q] Avbryt"
        };
        public static void UIGotoMainMenu(ToDo todo)
        {
            while (true)
            {
                _mainMenuOptions.ClearWriteLineToConsole();

                switch ("Välj: ".ForReadKey())
                {
                    case '1': // Lägg till en ny uppgift
                        UIGotoAddTaskMenu(todo);
                        break;
                    case '2': // Lista alla dagens uppgifter
                        UIPrintUnarchivedList(todo);
                        break;
                    case '3': // Lista alla akriverade uppgifter
                        UIPrintArchivedList(todo);
                        break;
                    case '4': // Arkivera alla avklarade uppgifter
                        PutTasksToArchive(todo);
                        break;
                    case 'q': // Avbryt
                        return;
                }
            }
        }
        static void UIGotoAddTaskMenu(ToDo todo)
        {
            string newTaskNote;
            List<TimelessTask> newTaskList = new List<TimelessTask>();

            while (true)
            {
                _addTaskMenuOptions.ClearWriteLineToConsole();

                switch ("Välj: ".ForReadKey())
                {
                    case '1': // Tidlös uppgift
                        newTaskNote = UIGetNewTaskNote(_noteLabel);
                        if (newTaskNote != null) // Användare har inte avbrutit
                        {
                            todo.TaskList.Add(new TimelessTask(newTaskNote));
                        }
                        return;
                    case '2': // Uppgift med deadline
                        DeadlineTask newDeadlineTask = CreateNewDeadlineTask();
                        if (newDeadlineTask != null) // Användare har inte avbrutit
                        {
                            todo.TaskList.Add(newDeadlineTask);
                        }
                        return;
                    case '3': // Uppgift med checklista
                        ChecklistTask newChecklistTask = CreateNewChecklistTask();
                        if (newChecklistTask != null) // Användare har inte avbrutit
                        {
                            todo.TaskList.Add(newChecklistTask);
                        }
                        return;
                    case 'q': // Avbryt
                        return;
                }
            }
        }
        static string UIGetNewTaskNote(string label)
        {
            bool isError = false;

            while (true)
            {
                label.ClearWriteLineToConsole();

                if (isError)
                {
                    "Du behöver ange minst en bokstav".WriteLineAsError();
                }

                string userInput = "Text: ".ForReadLine();

                if (userInput.ToLower() == _cancelWord)
                {
                    return null;
                }
                else if (String.IsNullOrEmpty(userInput))
                {
                    isError = true;
                }
                else
                {
                    return userInput;
                }
            };
        }
        static Deadline UIGetNewDeadline()
        {
            bool isError = false;
            string errorMessage = "";

            while (true)
            {
                "UPPGIFT MED DEADLINE".ClearWriteLineToConsole();
                $"När ska uppgiften vara färdig? Skriv in enligt modellen dd/mm/åååå. Eller skriv {_cancelWord} för att avbryta:".WriteLineToConsole();

                if (isError)
                {
                    errorMessage.WriteLineAsError();
                }

                string userInput = $"Text: ".ForReadLine();

                if (userInput.ToLower() == _cancelWord)
                {
                    return null;
                }

                else if (DateTime.TryParseExact(userInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    Deadline newDeadline = new Deadline(dateTime);

                    if (newDeadline.GetDaysTillDeadline() < 0)
                    {
                        isError = true;
                        errorMessage = "Datumet måste vara lika med eller högre än dagens datum.";
                    }
                    else
                    {
                        return newDeadline;
                    }
                }
                else
                {
                    isError = true;
                    errorMessage = "Du behöver skriva datumet enligt modellen";
                }
            }
        }
        static List<TimelessTask> UIGetNewTaskList()
        {
            List<TimelessTask> tempTaskList = new List<TimelessTask>();
            bool isError = false;
            string errorMessage = "";

            while (true)
            {
                $"Skriv in en uppgift du vill lägga till i checklistan, skriv {_escapeWord} när du inte vill lägga till mer.".WriteLineToConsole();

                if (isError)
                {
                    errorMessage.WriteLineAsError();
                    isError = false;
                }

                string userInput = "Text: ".ForReadLine();

                if (userInput.ToLower() == _escapeWord)
                {
                    if (tempTaskList.Count != 0)
                    {
                        return tempTaskList;
                    }
                    else
                    {
                        errorMessage = "Du behöver lägga till minst en uppgift till checklistan";
                        isError = true;
                    }
                }
                else if (String.IsNullOrEmpty(userInput))
                {
                    errorMessage = "Du behöver ange minst en bokstav";
                    isError = true;
                }
                else
                {
                    tempTaskList.Add(new TimelessTask(userInput));
                }
            }
        }
        static DeadlineTask CreateNewDeadlineTask()
        {
            string newTaskNote = UIGetNewTaskNote(_noteLabel);
            if (newTaskNote == null)
            {
                return null;
            }

            Deadline newDeadline = UIGetNewDeadline();
            if (newDeadline == null)
            {
                return null;
            }

            return new DeadlineTask(newTaskNote, newDeadline);
        }

        // Returnerar en ny checklista
        static ChecklistTask CreateNewChecklistTask()
        {
            string newTaskNote = UIGetNewTaskNote(_checklistLabel);
            if (newTaskNote == null)
            {
                return null;
            }

            List<TimelessTask> newTaskList = UIGetNewTaskList();
            if (newTaskList == null)
            {
                return null;
            }

            return new ChecklistTask(newTaskNote, newTaskList);
        }

        // Skriver ut information om alla uppgifter och kontrollerar användarens input
        static void UIMarkTaskAsDoneByIndex(List<Task> taskList)
        {
            bool isError = false;
            while (true)
            {
                "DAGENS UPPGIFTER".ClearWriteLineToConsole();
                taskList.WriteLineToConsole(true);

                if (isError)
                {
                    "Du har angett ett ogiltligt nummer.".WriteLineAsError();
                }

                $"Ange en uppgift du vill markera som färdig. Skriv {_escapeWord} när du är färdig".WriteLineToConsole();

                string userInput = "Välj: ".ForReadLine();

                if (userInput == _escapeWord)
                {
                    return;
                }

                if (taskList.TryCountBasedIndex(userInput, out Task task))
                {
                    if (task is ChecklistTask)
                    {
                        UIMarkChecklistTasksAsDone((ChecklistTask)task);
                    }
                    else
                    {
                        task.SetDone(true);
                    }

                    isError = false;
                }
                else
                {
                    isError = true;
                }

            }
        }

        // Hanterar checklistor och låter användaren interagera med sin lista
        static void UIMarkChecklistTasksAsDone(ChecklistTask checklistTask)
        {
            bool isError = false;

            while (true)
            {
                "DAGENS UPPGIFTER".ClearWriteLineToConsole();
                $"Du har angett checklistan med beskrivningen \"{checklistTask.Note}\". Välj en uppgift du vill bocka av eller skriv {_escapeWord} för att avsluta:".WriteLineToConsole();
                checklistTask.WriteLineToConsole(true, 4);

                if (isError)
                {
                    "Du har angett ett ogiltligt nummer.".WriteLineAsError();
                }

                string userInput = "Välj: ".ForReadLine();

                if (userInput == _escapeWord)
                {
                    return;
                }
                else if (checklistTask.TimelessTaskList.TryCountBasedIndex(userInput, out TimelessTask timelessTask))
                {
                    timelessTask.SetDone(true);

                    if (checklistTask.CheckIfAllTasksAreDone())
                    {
                        "Grattis! Allt är färdigt..".WriteLineToConsole();
                    }
                }
                else
                {
                    isError = true;
                }
            }
        }

        // Skriver ut information om oarkiverade uppgifter
        static void UIPrintUnarchivedList(ToDo todo)
        {
            "DAGENS UPPGIFTER".ClearWriteLineToConsole();
            List<Task> taskList = todo.GetAllUnarchivedTasks();

            if (taskList.Count == 0)
            {
                "Du har inga uppgifter idag!".WriteLineToConsole();
                "[Tryck vad som helst för att gå vidare]".ForReadKey();
            }
            else
            {
                UIMarkTaskAsDoneByIndex(taskList);
            }
        }

        // Skriver ut information om arkiverade uppgifter
        static void UIPrintArchivedList(ToDo todo)
        {
            "ARKIV".ClearWriteLineToConsole();
            List<Task> taskList = todo.GetAllArchivedTasks();

            if (taskList.Count == 0)
            {
                "Du har inga arkiverade uppgifter!".WriteLineToConsole();
                "[Tryck vad som helst för att gå vidare]".ForReadKey();
            }
            else
            {
                taskList.WriteLineToConsole(true);
                "[Tryck vad som helst för att gå vidare]".ForReadKey();
            }
        }

        // Ger information om uppgifter som arkiveras
        static void PutTasksToArchive(ToDo todo)
        {
            "ARKIVERADE UPPGIFTER".ClearWriteLineToConsole();
            int successAmount = todo.ArchiveAllDoneTasks();

            if (successAmount == 0)
            {
                "Alla uppgifter som går att arkivera är arkiverade!".WriteLineToConsole();
                "[Tryck vad som helst för att gå vidare]".ForReadKey();
            }
            else
            {
                $"Du har nu {successAmount}st uppgift(er) arkiverade!".WriteLineToConsole();
                "[Tryck vad som helst för att gå vidare]".ForReadKey();
            }
        }
    }
}