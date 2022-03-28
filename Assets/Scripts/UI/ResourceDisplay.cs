using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    private ResourceListSO resourceList;
    private Dictionary<ResourceSO, Transform> resourceTransformDictionary;

    private void Awake()
    {
        resourceList = Resources.Load<ResourceListSO>("ResourceList");
        Debug.Log(resourceList);
        resourceTransformDictionary = new Dictionary<ResourceSO, Transform>();  

        InitiateResourceDisplays();
    }

    private void Start()
    {
        UpdateAllResourceDisplay();
    }

    private void InitiateResourceDisplays()
    {
        // Finding and inactivate template
        Transform template = transform.Find("template");
        template.gameObject.SetActive(false);

        int index = 0;
        foreach (ResourceSO resource in resourceList.Resources)
        {
            Transform displayTransfrom = Instantiate(template, transform);
            displayTransfrom.gameObject.SetActive(true);

            var offsetAmount = -15;
            displayTransfrom.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, offsetAmount * index);

            resourceTransformDictionary[resource] = displayTransfrom;
            index++;
        }
    }

    public void UpdateAllResourceDisplay()
    { 
        foreach (ResourceSO resource in resourceList.Resources)
        {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resource);
            UpdateResourceDisplay(resource, resourceAmount);
        }
    }

    public void UpdateResourceDisplay(ResourceSO resourceType, int resourceAmount)
    {
        Transform resourceTransform = resourceTransformDictionary[resourceType];
        resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceType.name + ": " + resourceAmount.ToString());
    }
}
