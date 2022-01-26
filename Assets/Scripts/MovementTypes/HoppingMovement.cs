using UnityEngine;

public class HoppingMovement : IMovement
{
    private float sinX;

    public void Move(bool isMoving, float speed)
    {
        sinX += speed * Time.deltaTime;

    }
}
