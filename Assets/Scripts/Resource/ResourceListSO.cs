using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource List", menuName = "ScriptableObjects/ResourceList")]
public class ResourceListSO : ScriptableObject
{
    public List<ResourceSO> Resources;
}
