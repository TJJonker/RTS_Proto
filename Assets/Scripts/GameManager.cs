using Jonko.FunctionTimer;
using RTS.TaskSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RTSUnit;

    private void Start()
    {
        TaskSystem taskSystem = new TaskSystem();

        GameObject unit = SpawnRTSUnit(new Vector2(0, 0));

        WorkerTaskAI workerTaskAI = unit.AddComponent<WorkerTaskAI>();
        workerTaskAI.Setup(unit.GetComponent<RTSUnit>(), taskSystem);

        FunctionTimer.Create(() =>
        {
            TaskSystem.Task task = new TaskSystem.Task { targetPosition = new Vector2(3, 2) };
            taskSystem.AddTask(task);
        }, 3f);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
            //TaskSystem.Task task = new TaskSystem.Task { targetPosition = Mouse}
        //}
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
}
