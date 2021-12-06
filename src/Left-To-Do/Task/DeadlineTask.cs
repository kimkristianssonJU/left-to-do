using LeftToDo;

namespace ToDoTask
{
    public class DeadlineTask : Task
    {
        Deadline _deadline;
        public DeadlineTask(string note, Deadline deadline) : base(note)
        {
            this._deadline = deadline;
            this._note = note;
        }
        public Deadline Deadline
        {
            get { return this._deadline; }
        }
    }
}