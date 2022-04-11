using Jonko.Timers;
using Jonko.Utils;
using RTS.TaskSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RTSUnit;
    [SerializeField] private Sprite bloodSprite;
    [SerializeField] private Sprite stoneSprite;
    [SerializeField] private Sprite pixelSprite;

    private TaskSystem<Task> taskSystem;

    private StoneSlot stoneSlot;

    private void Start()
    {
        taskSystem = new TaskSystem<Task>();

        SpawnWorker(new Vector2(0, 0));
        //SpawnWorker(new Vector2(1, 1));
        //SpawnWorker(new Vector2(1.5f, 1));

        GameObject stoneSlotGameObject = SpawnStoneSlot(new Vector2(4, 2));
        stoneSlot = new StoneSlot(stoneSlotGameObject.transform);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GameObject stoneGameObject = SpawnStoneSprite(Utils.MouseToScreen(Mouse.current.position.ReadValue()));
            taskSystem.EnqueueTask(() =>
            {
                if (stoneSlot.IsEmpty())
                {
                    stoneSlot.SetHasItemIncoming(true);
                    Task task = new Task.TakeStoneToStoneSlot
                    {
                        stonePosition = stoneGameObject.transform.position,
                        stoneSlotPosition = stoneSlot.GetPosition(),
                        grabStone = (WorkerTaskAI worker) =>
                        {
                            stoneGameObject.transform.SetParent(worker.transform);
                        },
                        dropStone = () =>
                        {
                            stoneGameObject.transform.SetParent(null);
                            stoneSlot.SetStoneTransform(stoneGameObject.transform);
                        }
                    };
                    return task;
                }
                else return null;
            });
        }
        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SpawnStoneSlot(Utils.MouseToScreen(Mouse.current.position.ReadValue()));
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

    private class StoneSlot
    {
        private Transform stoneSlotTransform;
        private Transform stoneTransform;
        private bool hasItemIncoming;

        public StoneSlot(Transform stoneSlotTransform)
        {
            this.stoneSlotTransform = stoneSlotTransform;
            SetStoneTransform(null);
        }

        public void SetStoneTransform(Transform stoneTransform)
        {
            this.stoneTransform = stoneTransform;
            SetHasItemIncoming(false);
            UpdateSprite();

            if(stoneTransform != null)
            {
                FunctionTimer.Create(() =>
                {
                    Destroy(stoneTransform.gameObject);
                    SetStoneTransform(null);
                }, 3f);
            }
        }

        public bool IsEmpty() 
            => stoneTransform == null && !hasItemIncoming;

        public void SetHasItemIncoming(bool hasItemIncoming)
        {
            this.hasItemIncoming = hasItemIncoming;
            UpdateSprite();
        }

        public Vector2 GetPosition() 
            => stoneSlotTransform.position;

        private void UpdateSprite() 
            => stoneSlotTransform.GetComponent<SpriteRenderer>().color = IsEmpty() ? Color.white : Color.red;
    }

    public class Task : TaskBase
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

        public class TakeStoneToStoneSlot : Task
        {
            public Vector2 stonePosition;
            public Action<WorkerTaskAI> grabStone;
            public Vector2 stoneSlotPosition;
            public Action dropStone;
        }
    }
}
