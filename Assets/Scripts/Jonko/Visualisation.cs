using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jonko.Visualisation
{
    public class Visualisation
    {
        private static Sprite pixelSprite = Resources.Load<Sprite>("Pixel");         

        public static GameObject CreateBackground(Transform parent, Vector3 position, Vector3 size, Color color)
        {
            // Background object
            GameObject background = new GameObject("background");
            background.transform.parent = InputManager.Instance.transform;
            background.transform.position = position;
            background.transform.localScale = size;

            // Background sprite
            GameObject sprite = new GameObject("sprite");
            sprite.transform.parent = background.transform;
            sprite.transform.position = new Vector3(.5f, .5f, 0);

            var sr = sprite.AddComponent<SpriteRenderer>();
            sr.sprite = pixelSprite;

            return background;
        }

        public static GameObject CreateBackgroundGUI(Transform parent, Vector3 position, Vector3 size, Color color)
        {
            // Canvas
            GameObject canvas = new GameObject("Canvas");
            var canvasObject = canvas.AddComponent<Canvas>();
            canvasObject.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.GetComponent<RectTransform>().SetParent(parent);

            // Background
            GameObject background = new GameObject("background");
            
            var image = background.AddComponent<Image>();
            var rectTransform = background.GetComponent<RectTransform>();

            rectTransform.SetParent(canvas.transform);

            rectTransform.pivot = Vector2.zero;
            rectTransform.localPosition = new Vector2(-Screen.width/2, -Screen.height/2) + (Vector2)position;
            rectTransform.sizeDelta = size;

            image.color = color;

            return background;
        }

        public static TextMeshProUGUI CreateTextFieldGUI(Transform parent, Vector2 position, string standardText = "")
        {
            GameObject textObject = new GameObject("text");
            TextMeshProUGUI textField;
            textField = textObject.AddComponent<TextMeshProUGUI>();
            textField.fontStyle = FontStyles.Bold;
            textField.SetText(standardText);
            textField.alignment = TextAlignmentOptions.Left;
            textField.enableWordWrapping = false;

            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent);
            rectTransform.pivot = Vector2.zero;
            rectTransform.position = new Vector2(0, 0) + position;
            rectTransform.sizeDelta = Vector2.zero;


            return textField;
        }
    }
}
