using UnityEngine;

namespace Jonko.Visualisation
{
    public class Visualisation 
    {
        public static TextMesh CreateWorldText(string text, Color color, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left)
        {
            var scaling = 10;
            GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = Vector3.one / scaling;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment; 
            textMesh.text = text;
            textMesh.fontSize = fontSize * scaling;
            textMesh.color = color;
            return textMesh;
        }
    }
}
