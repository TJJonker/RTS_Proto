using UnityEngine;
using UnityEngine.InputSystem.Controls;

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
    }

    public static class Logic
    {
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
    }
}
