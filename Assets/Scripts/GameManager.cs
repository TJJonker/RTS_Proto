using Jonko.Timers;
using Jonko.Utils;
using RTS.TaskSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RTSUnit;
    [SerializeField] private Sprite bloodSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite pixelSprite;

    private TaskSystem taskSystem;

    private void Start()
    {
        taskSystem = new TaskSystem();

        SpawnWorker(new Vector2(0, 0));
        //SpawnWorker(new Vector2(1, 1));
        //SpawnWorker(new Vector2(1.5f, 1));

    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GameObject blood = SpawnBlood(Utils.MouseToScreen(Mouse.current.position.ReadValue()));
            SpriteRenderer spriteRenderer = blood.GetComponent<SpriteRenderer>();

            float waitTime = Time.time;
            taskSystem.EnqueueTask(() =>
            {
                if (Time.time >= waitTime)
                {
                    TaskSystem.Task.BloodCleanUp task = new TaskSystem.Task.BloodCleanUp
                    {
                        targetPosition = blood.transform.position,
                        cleanUpAction = () =>
                        {
                            float alpha = 1f;
                            FunctionUpdater.Create(() =>
                            {
                                alpha -= Time.deltaTime;
                                spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
                                if (alpha <= 0f) return true;
                                else return false;
                            });
                        }
                    };
                    return task;
                }
                else return null;
            });
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

    private GameObject SpawnStoneSprite(Vector2 position)
    {
        GameObject gameObject = new GameObject("StoneSprite", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = stoneSprite;
        gameObject.transform.position = position;
        return gameObject;
    }

    private GameObject SpawnStoneSlot(Vector2 position)
    {
        GameObject gameObject = new GameObject("StoneSlot", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = pixelSprite;
        gameObject.transform.position = position;
        gameObject.transform.localScale = new Vector3(3, 3);
        return gameObject;
    }
}
