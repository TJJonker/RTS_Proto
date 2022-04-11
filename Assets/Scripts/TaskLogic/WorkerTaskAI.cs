using Jonko.Timers;
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
                if(value == WorkerState.WaitingForNextTask)
                    timer.Unpause();
                else timer.Pause();
                state = value;
            }
        }
        private WorkerState state;

        private RTSUnit worker;
        private TaskSystem<GameManager.Task> taskSystem;
        private FunctionTimer timer;

        /// <summary>
        ///     Serves as a constructor after the component is added to an object
        /// </summary>
        /// <param name="worker"> The RTSUnit of the worker </param>
        /// <param name="taskSystem"> The task system responsible for the tasks </param>
        public void Setup(RTSUnit worker, TaskSystem<GameManager.Task> taskSystem)
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
            GameManager.Task task = taskSystem.RequestNextTask();

            if (task == null)
            {
                State = WorkerState.WaitingForNextTask;
            }
            else
            {
                State = WorkerState.ExecutingTask;
                if(task is GameManager.Task.MoveToPosition)
                    ExecuteTask_MoveToPosition(task as GameManager.Task.MoveToPosition);
                if (task is GameManager.Task.Victory)
                    ExecuteTask_Victory(task as GameManager.Task.Victory);
                if (task is GameManager.Task.BloodCleanUp)
                    ExecuteTask_BloodCleanUp(task as GameManager.Task.BloodCleanUp);
                if (task is GameManager.Task.TakeStoneToStoneSlot)
                    ExecuteTask_TakeStoneToStoneSlot(task as GameManager.Task.TakeStoneToStoneSlot);
            }
        }

        /// <summary>
        ///     Function that makes sure the given task will be executed.
        /// </summary>
        /// <param name="MoveToPositionTask"> The task that should be executed. </param>
        private void ExecuteTask_MoveToPosition(GameManager.Task.MoveToPosition MoveToPositionTask)
        {
            worker.MoveTo(MoveToPositionTask.targetPosition, () => State = WorkerState.WaitingForNextTask);
        }

        private void ExecuteTask_Victory(GameManager.Task.Victory victory)
        {
            worker.VictoryDance(() => State = WorkerState.WaitingForNextTask); 
        }

        private void ExecuteTask_BloodCleanUp(GameManager.Task.BloodCleanUp bloodCleanUp)
        {
            worker.MoveTo(bloodCleanUp.targetPosition, () =>
            {
                worker.VictoryDance(() =>
                {
                    bloodCleanUp.cleanUpAction();
                    State = WorkerState.WaitingForNextTask;
                });
            });
        }

        private void ExecuteTask_TakeStoneToStoneSlot(GameManager.Task.TakeStoneToStoneSlot takeToStoneSlotTask)
        {
            worker.MoveTo(takeToStoneSlotTask.stonePosition, () =>
            {
                takeToStoneSlotTask.grabStone(this);
                worker.MoveTo(takeToStoneSlotTask.stoneSlotPosition, () =>
                {
                    takeToStoneSlotTask.dropStone();
                    State = WorkerState.WaitingForNextTask;
                });
            });
        }
    }
}
