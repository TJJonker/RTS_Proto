using Jonko.Timers;
using UnityEngine;

namespace RTS.TaskSystem
{
    public class TranspoterTaskAI: MonoBehaviour
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
                if(value == WorkerState.WaitingForNextTask)
                    timer.Unpause();
                else timer.Pause();
                state = value;
            }
        }
        private WorkerState state;

        private RTSUnit worker;
        private TaskSystem<GameManager.TransporterTask> taskSystem;
        private FunctionTimer timer;

        /// <summary>
        ///     Serves as a constructor after the component is added to an object
        /// </summary>
        /// <param name="worker"> The RTSUnit of the worker </param>
        /// <param name="taskSystem"> The task system responsible for the tasks </param>
        public void Setup(RTSUnit worker, TaskSystem<GameManager.TransporterTask> taskSystem)
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
            GameManager.TransporterTask task = taskSystem.RequestNextTask();

            if (task == null)
            {
                State = WorkerState.WaitingForNextTask;
            }
            else
            {
                State = WorkerState.ExecutingTask;
                if(task is GameManager.TransporterTask.TakeItemFromSlotToPosition)
                    ExecuteTask_TakeItemFromSlotToPosition(task as GameManager.TransporterTask.TakeItemFromSlotToPosition);
            }
        }

        /// <summary>
        ///     Function that makes sure the given task will be executed.
        /// </summary>
        /// <param name="MoveToPositionTask"> The task that should be executed. </param>
        private void ExecuteTask_TakeItemFromSlotToPosition(GameManager.TransporterTask.TakeItemFromSlotToPosition task)
        {
            worker.MoveTo(task.itemSlotPosition, () =>
            {
                task.grabItem(this);
                worker.MoveTo(task.targetPosition, () =>
                {
                    task.dropItem();
                    State = WorkerState.WaitingForNextTask;
                });
            });
        }
    }
}
