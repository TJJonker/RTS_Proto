using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;

public class UnitMoveOrderSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Entities.ForEach((Entity entity, ref Translation translation) =>
            {
                EntityManager.AddComponentData(entity, new PathfindingParams
                {
                    startPosition = new int2(0, 0),
                    endPosition = new int2(4, 0)
                });
            });
        }
    }
}
