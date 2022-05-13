using Unity.Entities;
using UnityEngine;

public class IntBuffElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public int[] valueArray;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        DynamicBuffer<IntBufferElement> dynamicBuffer = dstManager.AddBuffer<IntBufferElement>(entity);
        foreach (int value in valueArray)
            dynamicBuffer.Add(new IntBufferElement { Value = value });
    }
}
