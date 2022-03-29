using System;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer
{

    private static MonoBehaviourHook monoGameObject;

    public static FunctionTimer Create(Action action, float timer)
    {
        InitIfNeeded();

        FunctionTimer functionTimer = new FunctionTimer(action, timer);
        monoGameObject.AddTimer(functionTimer);
        
        return functionTimer;
    }




    private class MonoBehaviourHook : MonoBehaviour
    {
        private List<FunctionTimer> activeTimerList;
        private List<FunctionTimer> timerToRemove;

        public MonoBehaviourHook()
        {
            activeTimerList = new List<FunctionTimer>();
            timerToRemove = new List<FunctionTimer>();  
        }

        private void Update() {
            foreach (FunctionTimer timer in activeTimerList) timer.Update();
            RemoveTimers();
        }

        public void AddTimer(FunctionTimer timer) => activeTimerList.Add(timer);        
        public void RemoveTimer(FunctionTimer timer) => timerToRemove.Add(timer);
        private void RemoveTimers()
        {
            if (timerToRemove.Count > 0) {
                foreach (FunctionTimer timer in timerToRemove)
                    activeTimerList.Remove(timer);
                timerToRemove.Clear();
            }
        }
    }


    private static void InitIfNeeded() 
    {
        if (monoGameObject == null)
        {
            GameObject gameObject = new GameObject("FunctionOnTimer");
            monoGameObject = gameObject.AddComponent<MonoBehaviourHook>();
        }
    }






    private Action action;
    private float timer;

    private FunctionTimer(Action action, float timer)
    {
        this.action = action;
        this.timer = timer; 
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            action();
            DestroySelf();
        }
    }

    private void DestroySelf() => monoGameObject.RemoveTimer(this);
}
