using Unity.Entities;
using UnityEngine;

public class ECSTesting : MonoBehaviour
{
    private void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = entityManager.CreateEntity();

        // DynamicBuffer<IntBufferElement> dynamicBuffer = entityManager.AddBuffer<IntBufferElement>(entity);

        entityManager.AddBuffer<IntBufferElement>(entity);
        DynamicBuffer<IntBufferElement> dynamicBuffer = entityManager.GetBuffer<IntBufferElement>(entity);

        dynamicBuffer.Add(new IntBufferElement { Value = 1 });
        dynamicBuffer.Add(new IntBufferElement { Value = 3 });
        dynamicBuffer.Add(new IntBufferElement { Value = 2 });

        DynamicBuffer<int> intDynamicBuffer = dynamicBuffer.Reinterpret<int>();
        intDynamicBuffer[1] = 5;

        for (int i = 0; i < intDynamicBuffer.Length; i++)
            intDynamicBuffer[i]++;
    }
}
