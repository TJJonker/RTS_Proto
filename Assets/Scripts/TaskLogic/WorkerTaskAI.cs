using Jonko.FunctionTimer;
using UnityEngine;

namespace RTS.TaskSystem
{
    public class WorkerTaskAI : MonoBehaviour
    {
        private enum WorkerState
        {
            WaitingForNextTask,
            ExecutingTask,
        }

        /// <summary>
        ///     Determines the state of the worker
        /// </summary>
        private WorkerState State
        {
            get => state;
            set
            {
                if(state == WorkerState.WaitingForNextTask)
                    timer.Unpause();
                else timer.Pause();
                state = value;
            }
        }
        private WorkerState state;

        private RTSUnit worker;
        private TaskSystem taskSystem;
        private FunctionTimer timer;

        /// <summary>
        ///     Serves as a constructor after the component is added to an object
        /// </summary>
        /// <param name="worker"> The RTSUnit of the worker </param>
        /// <param name="taskSystem"> The task system responsible for the tasks </param>
        public void Setup(RTSUnit worker, TaskSystem taskSystem)
        {
            this.worker = worker;
            this.taskSystem = taskSystem;

            timer = FunctionTimer.Create(RequestNextTask, .2f, true);
            State = WorkerState.WaitingForNextTask;
        }

        /// <summary>
        ///     Requests the next task from the task system and changes state accordingly
        /// </summary>
        private void RequestNextTask()
        {
            TaskSystem.Task task = taskSystem.RequestNextTask();

            if (task == null)
            {
                State = WorkerState.WaitingForNextTask;
            }
            else
            {
                state = WorkerState.ExecutingTask;
                ExecuteTask(task);
            }
        }

        /// <summary>
        ///     Function that makes sure the given task will be executed.
        /// </summary>
        /// <param name="task"> The task that should be executed. </param>
        private void ExecuteTask(TaskSystem.Task task)
        {
            worker.MoveTo(task.targetPosition, () => state = WorkerState.WaitingForNextTask);
        }
    }
}
