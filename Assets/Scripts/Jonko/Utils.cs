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
}
