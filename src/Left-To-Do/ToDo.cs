using System;
using System.Collections.Generic;
using ToDoTask;

namespace LeftToDo
{
    public class ToDo
    {
        // Tasklists
        List<Task> _taskList = new List<Task>();

        // Properties
        public List<Task> TaskList { get => _taskList; }

        public int ArchiveAllDoneTasks()
        {
            int successCount = 0;
            foreach (Task task in this._taskList)
            {
                if (task.IsDone)
                {
                    task.SetArchived(true);
                    successCount++;
                }
            }
            return successCount;
        }
        public List<Task> GetAllArchivedTasks()
        {
            List<Task> tempTaskList = new List<Task>();

            foreach (Task task in this._taskList)
            {
                if (task.IsArchived)
                {
                    tempTaskList.Add(task);
                }
            }

            return tempTaskList;
        }
        public List<Task> GetAllUnarchivedTasks()
        {
            List<Task> tempTaskList = new List<Task>();

            foreach (Task task in this._taskList)
            {
                if (!task.IsArchived)
                {
                    tempTaskList.Add(task);
                }
            }

            return tempTaskList;

        }
    }
}