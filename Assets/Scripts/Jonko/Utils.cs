using System;
using UnityEngine;

namespace Jonko.Utils
{
    public static class Utils 
    {        
        static Camera mainCamera = GameObject.FindObjectOfType<Camera>();

        /// <summary>
        ///     Returns the world position of the current position of the mouse.
        /// </summary>
        /// <param name="mousePosition"> Position of the mouse on the screen </param>
        /// <returns> World position of the mouse </returns>
        public static Vector3 MouseToScreen(Vector3 mousePosition)
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            mouseWorldPosition.z = 0f;
            return mouseWorldPosition;
        }
<<<<<<< Updated upstream
=======

        /// <summary>
        ///     Logical operator that checks if one of the two float is positive and the other negative
        /// </summary>
        /// <param name="i1"> first float </param>
        /// <param name="i2"> second float </param>
        /// <returns> Whether one of the floats is positive and the other is negative. </returns>
        public static bool OppositePositiveNegative(float i1, float i2)
        {
            return i1 > 0 && i2 < 0 || i1 < 0 && i2 > 0;
        }
>>>>>>> Stashed changes
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
