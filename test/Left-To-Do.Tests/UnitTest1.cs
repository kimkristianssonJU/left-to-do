using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using ToDoTask;

namespace LeftToDo.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void TestIfArchivingIsPossible()
        {
            // Arrange
            ToDo todo = new ToDo();
            List<Task> taskList = new List<Task>();
            todo.TaskList.Add(new TimelessTask("OK"));
            todo.TaskList.Add(new DeadlineTask("OK", new Deadline(new DateTime(2021, 12, 9))));
            todo.TaskList.Add(new ChecklistTask("OK9", new List<TimelessTask>() { new TimelessTask("ok0"), new TimelessTask("ok1"), new TimelessTask("ok2") }));
            SetToDone(todo.TaskList);

            // Act
            todo.ArchiveAllDoneTasks();

            // Assert
            Assert.True(todo.TaskList[0].IsArchived);
            Assert.True(todo.TaskList[1].IsArchived);
            Assert.True(todo.TaskList[2].IsArchived);
        }

        void SetToDone(List<Task> list)
        {
            list[0].SetDone(true);
            list[1].SetDone(true);
            ChecklistTask checklistTask = (ChecklistTask)list[2];
            checklistTask.TimelessTaskList[0].SetDone(true);
            checklistTask.TimelessTaskList[1].SetDone(true);
            checklistTask.TimelessTaskList[2].SetDone(true);
            checklistTask.CheckIfAllTasksAreDone();
        }

        [Fact]
        public void TestIfSetToDoneIsPossible()
        {
            // Arrange
            ToDo todo = new ToDo();
            List<Task> taskList = new List<Task>();
            todo.TaskList.Add(new TimelessTask("OK"));
            todo.TaskList.Add(new DeadlineTask("OK", new Deadline(new DateTime(2021, 12, 9))));
            todo.TaskList.Add(new ChecklistTask("OK9", new List<TimelessTask>() { new TimelessTask("ok0"), new TimelessTask("ok1"), new TimelessTask("ok2") }));

            // Act
            todo.TaskList[0].SetDone(true);
            todo.TaskList[1].SetDone(true);
            ChecklistTask checklistTask = (ChecklistTask)todo.TaskList[2];
            checklistTask.TimelessTaskList[0].SetDone(true);
            checklistTask.TimelessTaskList[1].SetDone(true);
            checklistTask.TimelessTaskList[2].SetDone(true);
            checklistTask.CheckIfAllTasksAreDone();

            // Assert
            Assert.True(todo.TaskList[0].IsDone);
            Assert.True(todo.TaskList[1].IsDone);
            Assert.True(todo.TaskList[2].IsDone);
        }

        [Fact]
        public void TestIfIdentifyDifferentTasksIsPossible()
        {
            // Arrange
            List<Task> taskList = new List<Task>();
            taskList.Add(new TimelessTask("Tvätta"));
            taskList.Add(new DeadlineTask("Laga fläkten", new Deadline(DateTime.Now)));
            taskList.Add(new ChecklistTask("Betala hyran", new List<TimelessTask>()));

            // Act
            bool isTimelessTask = taskList[0] is TimelessTask;
            bool isDeadlineTask = taskList[1] is DeadlineTask;
            bool isChecklistTask = taskList[2] is ChecklistTask;
            bool isNotChecklistTask = taskList[2] is TimelessTask;

            // Assert
            Assert.True(isTimelessTask);
            Assert.True(isTimelessTask);
            Assert.True(isTimelessTask);
            Assert.False(isNotChecklistTask);
        }

        [Fact]
        public void TestIfDateTimeTryParseExactWorks()
        {
            // Arrange
            // Skiljer 7 dagar mellan dagarna
            bool isParsed = DateTime.TryParseExact("09/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstDate);
            DateTime secondDate = DateTime.ParseExact("16/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int expectedDays = 7; // 7 dagar

            // Act
            TimeSpan daysLeft = secondDate - firstDate;

            // Assert
            Assert.True(isParsed);
            Assert.Equal(expectedDays, (int)Math.Ceiling(daysLeft.TotalDays));
        }

        [Fact]
        public void TestIfAllTasksAreDoneInChecklistTask()
        {
            // Arrange
            List<TimelessTask> timelessTaskList = new List<TimelessTask>();
            timelessTaskList.Add(new TimelessTask("Tvätta"));
            timelessTaskList.Add(new TimelessTask("Laga fläkten"));
            timelessTaskList.Add(new TimelessTask("Betala hyran"));

            ChecklistTask checklistTask = new ChecklistTask("Hushållssysslor", timelessTaskList);

            // Act
            foreach (TimelessTask task in checklistTask.TimelessTaskList)
            {
                task.SetDone(true);
            }
            checklistTask.CheckIfAllTasksAreDone();

            // Assert
            Assert.True(checklistTask.IsDone);
        }

        [Fact]
        public void TestIfAddingNewChecklistIsPossible()
        {
            // Arrange
            List<TimelessTask> timelessTaskList = new List<TimelessTask>();
            timelessTaskList.Add(new TimelessTask("Tvätta"));
            timelessTaskList.Add(new TimelessTask("Laga fläkten"));
            timelessTaskList.Add(new TimelessTask("Betala hyran"));
            ChecklistTask checklistTask = new ChecklistTask("Hushållssysslor", timelessTaskList);

            // Act
            string expectedOutput = "Laga fläkten";
            string taskOutput = checklistTask.TimelessTaskList[1].Note;

            // Assert
            Assert.Equal(expectedOutput, taskOutput);
        }

        [Fact]
        public void TestIfPossibleToCreateDeadlineTask()
        {
            // Arrange
            DateTime endDate = new DateTime(2021, 12, 9);
            Deadline deadline = new Deadline(endDate);
            DeadlineTask task = new DeadlineTask("Tvätta", deadline);

            DateTime currentDate = DateTime.Now;

            // Act
            TimeSpan timeSpan = endDate - currentDate;
            int expectedDays = (int)Math.Ceiling(timeSpan.TotalDays);

            // Assert
            Assert.Equal(expectedDays, task.Deadline.GetDaysTillDeadline());
        }

        [Fact]
        public void TestDateHowManyDaysRemaining()
        {
            // Arrange
            // Skiljer 7 dagar mellan dagarna
            DateTime firstDate = DateTime.ParseExact("09/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime secondDate = DateTime.ParseExact("16/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int expectedDays = 7; // 7 dagar

            // Act
            TimeSpan daysLeft = secondDate - firstDate;


            // Assert
            Assert.Equal(expectedDays, (int)Math.Ceiling(daysLeft.TotalDays));
        }
    }
}
