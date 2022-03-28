using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    
    public Dictionary<ResourceSO, int> resourceDictionary;

    private void Awake()
    {
        Instance = this;
        resourceDictionary = new Dictionary<ResourceSO, int>(); 
    }

    public void AddResource(ResourceSO resource, int amount) => resourceDictionary[resource] += amount;
    public void RemoveResource(ResourceSO resource, int amount) => resourceDictionary[resource] -= amount;

    public int GetResourceAmount(ResourceSO resource)
    {
        if(resourceDictionary.ContainsKey(resource))
            return resourceDictionary[resource];
        return 0;
    }
}
