namespace ToDoTask
{
    public class TimelessTask : Task
    {
        public TimelessTask(string note) : base(note)
        {
            this._note = note;
        }
    }
}