using Jonko.FunctionTimer;
using Jonko.Utils;
using RTS.TaskSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RTSUnit;
    private TaskSystem taskSystem = new TaskSystem();

    private void Start()
    {
        SpawnWorker(new Vector2(0, 0));
        SpawnWorker(new Vector2(1, 1));

        FunctionTimer.Create(() =>
        {
            TaskSystem.Task task = new TaskSystem.Task { targetPosition = new Vector2(3, 2) };
            taskSystem.AddTask(task);
        }, 2f);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TaskSystem.Task task = new TaskSystem.Task { targetPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue()) };
            taskSystem.AddTask(task);
        }
    }

    /// <summary>
    ///     Spawns an RTS Unit at the given position
    /// </summary>
    /// <param name="position"> The position the RTS Unit should spawn </param>
    /// <returns> The RTS Unit GameObject </returns>
    public GameObject SpawnRTSUnit(Vector2 position)
    {
        GameObject unit = Instantiate(RTSUnit);
        unit.transform.position = position;
        return unit;
    }

    public GameObject SpawnWorker(Vector2 position)
    {
        GameObject unit = SpawnRTSUnit(position);

        WorkerTaskAI workerTaskAI = unit.AddComponent<WorkerTaskAI>();
        workerTaskAI.Setup(unit.GetComponent<RTSUnit>(), taskSystem);
        return unit;
    }
}
