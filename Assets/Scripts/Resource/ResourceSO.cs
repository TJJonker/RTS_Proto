using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource")]
public class ResourceSO : ScriptableObject
{
    public string resourceName;
    public Sprite sprite;
}
