using UnityEngine;

namespace Jonko.Visualisation
{
    public class Visualisation 
    {
        public static TextMesh CreateWorldText(string text, Color color, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40)
        {
            GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            return textMesh;
        }
    }
}
