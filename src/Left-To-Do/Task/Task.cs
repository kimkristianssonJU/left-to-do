using LeftToDo;

namespace ToDoTask
{
    public abstract class Task
    {
        protected string _note;
        protected bool _isArchived = false;
        protected bool _isDone = false;
        protected Task(string note)
        {
            this._note = note;
        }

        public bool IsArchived { get => _isArchived; }
        public bool IsDone { get => _isDone; }
        public string Note { get => _note; }
        public void SetDone(bool isDone)
        {
            this._isDone = isDone;
        }
        public void SetArchived(bool isArchived)
        {
            this._isArchived = isArchived;
        }
    }
}