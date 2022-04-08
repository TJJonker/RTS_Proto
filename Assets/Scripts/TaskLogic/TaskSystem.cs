using Jonko.Timers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.TaskSystem 
{
    public class TaskSystem
    {
        public abstract class Task
        {
            public class MoveToPosition : Task
            {
                public Vector3 targetPosition;
            }

            public class Victory : Task
            {

            }

            public class BloodCleanUp : Task
            {
                public Vector3 targetPosition;
                public Action cleanUpAction;
            }
        }

        public class QueuedTask
        {
            private Func<Task> tryGetTaskFunc;

            public QueuedTask(Func<Task> tryGetTaskFunc)
            {
                this.tryGetTaskFunc = tryGetTaskFunc;
            }

            public Task TryDequeueTask() => tryGetTaskFunc();
        }

        private List<Task> taskList;
        private List<QueuedTask> queuedTaskList;   

        public TaskSystem()
        {
            taskList = new List<Task>();
            queuedTaskList = new List<QueuedTask>();
            FunctionTimer.Create(DequeueTasks, .2f, true);
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

        public void EnqueueTask(QueuedTask queuedTask) => queuedTaskList.Add(queuedTask);
        public void EnqueueTask(Func<Task> tryGetTaskFunc)
        {
            QueuedTask queuedTask = new QueuedTask(tryGetTaskFunc);
            queuedTaskList.Add(queuedTask);
        }

        private void DequeueTasks()
        {
            for(int i = 0; i < queuedTaskList.Count; i++)  
            {
                QueuedTask queuedTask = queuedTaskList[i];
                Task task = queuedTask.TryDequeueTask();
                if (task != null)
                {
                    AddTask(task);
                    queuedTaskList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
