using Unity.Entities;

public class TestBufferFromEntitySystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<tag_bob>().ForEach((Entity bobEntity) =>
        {
            // Get buffer from Bob tagged entity
            BufferFromEntity<IntBufferElement> intBufferFromEntity = GetBufferFromEntity<IntBufferElement>();

            // Look for an entity with the alice tag
            Entity aliceEntity = Entity.Null;
            Entities.WithAll<tag_alice>().ForEach((Entity entity) => aliceEntity = entity);

            // Get intBufferElement from the alice tagged entity
            DynamicBuffer<IntBufferElement> aliceDynamicBuffer = intBufferFromEntity[aliceEntity];

            // Change the value of the alice tagged entitys' intBufferElement
            for(int i = 0; i < aliceDynamicBuffer.Length; i++)
            {
                IntBufferElement intBufferElement = aliceDynamicBuffer[i];
                intBufferElement.Value++;
                aliceDynamicBuffer[i] = intBufferElement;
            }
        });
    }
}
