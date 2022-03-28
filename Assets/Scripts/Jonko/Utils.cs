using System;
using UnityEngine;

namespace Jonko.Utils
{
    public static class Utils 
    {        
        static Camera mainCamera = GameObject.FindObjectOfType<Camera>();

        public static Vector3 MouseToScreen(Vector3 mousePosition)
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            mouseWorldPosition.z = 0f;
            return mouseWorldPosition;
        }
    }

    public class TimeBasedAction
    {
        private Action action;
        private float timer;
        private float timerMax;
        private bool repeat;
        private bool isActive;

        public TimeBasedAction(Action action, float timerMax, bool repeat = false)
        {
            this.action = action;
            this.timerMax = timerMax;
            this.repeat = repeat;

            timer = timerMax;
            isActive = true;
        }

        public void UpdateTimer()
        {
            if (!isActive) return;
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if(timer <= 0f)
            {
                action.Invoke();
                if(repeat) timer += timerMax;
            }
        }

        public void SetActive(bool active) => isActive = active;




    }

}
