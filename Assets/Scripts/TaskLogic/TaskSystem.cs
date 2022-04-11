using Jonko.Timers;
using System;
using System.Collections.Generic;

namespace RTS.TaskSystem 
{
    public class TaskSystem<TTask> where TTask : TaskBase
    {

        private List<TTask> taskList;
        private List<QueuedTask<TTask>> queuedTaskList;   

        public TaskSystem()
        {
            taskList = new List<TTask>();
            queuedTaskList = new List<QueuedTask<TTask>>();
            FunctionTimer.Create(DequeueTasks, .2f, true);
        }

        /// <summary>
        ///     Requests next task in the task list.
        /// </summary>
        /// <returns> Requested task </returns>
        public TTask RequestNextTask()
        {
            if (taskList.Count > 0)
            {
                TTask task = taskList[0];
                taskList.RemoveAt(0);
                return task;
            }
            else return null;
        }

        /// <summary>
        ///     Adds a task to the task list.
        /// </summary>
        /// <param name="task"> Task to add </param>
        public void AddTask(TTask task) => taskList.Add(task);

        public void EnqueueTask(QueuedTask<TTask> queuedTask) => queuedTaskList.Add(queuedTask);
        public void EnqueueTask(Func<TTask> tryGetTaskFunc)
        {
            QueuedTask<TTask> queuedTask = new QueuedTask<TTask>(tryGetTaskFunc);
            queuedTaskList.Add(queuedTask);
        }

        private void DequeueTasks()
        {
            for(int i = 0; i < queuedTaskList.Count; i++)  
            {
                QueuedTask<TTask> queuedTask = queuedTaskList[i];
                TTask task = queuedTask.TryDequeueTask();
                if (task != null)
                {
                    AddTask(task);
                    queuedTaskList.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public abstract class TaskBase { }

    public class QueuedTask<TTask> where TTask : TaskBase
    {
        private Func<TTask> tryGetTaskFunc;

        public QueuedTask(Func<TTask> tryGetTaskFunc)
            => this.tryGetTaskFunc = tryGetTaskFunc;

        public TTask TryDequeueTask() => tryGetTaskFunc();
    }
}
