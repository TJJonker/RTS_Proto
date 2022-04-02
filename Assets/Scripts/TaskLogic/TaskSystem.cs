using System.Collections.Generic;
using UnityEngine;

namespace RTS.TaskSystem 
{
    public class TaskSystem
    {
        public class Task
        {
            public Vector3 targetPosition;
        }

        private List<Task> taskList;

        public TaskSystem()
        {
            taskList = new List<Task>();
        }

        /// <summary>
        ///     Requests next task in the task list.
        /// </summary>
        /// <returns> Requested task </returns>
        public Task RequestNextTask()
        {
            if (taskList.Count > 0)
            {
                Task task = taskList[0];
                taskList.RemoveAt(0);
                return task;
            }
            else return null;
        }

        /// <summary>
        ///     Adds a task to the task list.
        /// </summary>
        /// <param name="task"> Task to add </param>
        public void AddTask(Task task) => taskList.Add(task);
    }
}
