using Unity.Entities;
using UnityEngine;

public class ECSTesting : MonoBehaviour
{
    private void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = entityManager.CreateEntity();
        DynamicBuffer<IntBufferElement> dynamicBuffer = entityManager.AddBuffer<IntBufferElement>(entity);
        dynamicBuffer.Add(new IntBufferElement { Value = 1 });
        dynamicBuffer.Add(new IntBufferElement { Value = 3 });
        dynamicBuffer.Add(new IntBufferElement { Value = 2 });
    }
}
