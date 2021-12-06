using System;

namespace LeftToDo
{
    public class Deadline
    {
        DateTime _finalDate;
        public Deadline(DateTime finalDate)
        {
            this._finalDate = finalDate;
        }
        public int GetDaysTillDeadline()
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan timeSpan = this._finalDate - currentDate;
            int daysTillDeadline = (int)Math.Ceiling(timeSpan.TotalDays);

            return daysTillDeadline;
        }
    }
}