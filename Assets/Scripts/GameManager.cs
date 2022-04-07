using Jonko.Timers;
using Jonko.Utils;
using RTS.TaskSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RTSUnit;
    [SerializeField] private Sprite bloodSprite;

    private TaskSystem taskSystem = new TaskSystem();

    private void Start()
    {
        SpawnWorker(new Vector2(0, 0));
        //SpawnWorker(new Vector2(1, 1));

    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GameObject blood = SpawnBlood(Utils.MouseToScreen(Mouse.current.position.ReadValue()));
            TaskSystem.Task.BloodCleanUp task = new TaskSystem.Task.BloodCleanUp {
                targetPosition = blood.transform.position,
                cleanUpAction = () => Object.Destroy(blood.gameObject)                
            };
            taskSystem.AddTask(task);   
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            TaskSystem.Task task = new TaskSystem.Task.Victory();
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

    public GameObject SpawnBlood(Vector2 position)
    {
        GameObject gameObject = new GameObject("BloodSplatter", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = bloodSprite;
        gameObject.transform.position = position;   
        return gameObject;
    }
}
