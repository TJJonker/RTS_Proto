using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private ResourceSO resourceType;
    [Tooltip("Amount of workers that can work at this container")]
    [SerializeField, Range(1, 5)] private int amountOfWorkers;
    [Tooltip("Amount of seconds to harvest a single unit")]
    [SerializeField] private float harvestSpeed;

}
