using System.Collections.Generic;

namespace ToDoTask
{
    public class ChecklistTask : Task
    {
        List<TimelessTask> _timelessTaskList;
        public ChecklistTask(string note, List<TimelessTask> timelessTaskList) : base(note)
        {
            this._timelessTaskList = timelessTaskList;
        }
        public List<TimelessTask> TimelessTaskList
        {
            get { return this._timelessTaskList; }
        }
        public bool CheckIfAllTasksAreDone()
        {
            foreach (TimelessTask task in this._timelessTaskList)
            {
                if (!task.IsDone)
                {
                    return false;
                }
            }

            this.SetDone(true);
            return true;
        }
    }
}