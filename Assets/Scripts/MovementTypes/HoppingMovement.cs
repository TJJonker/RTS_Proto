using UnityEngine;

public class HoppingMovement : IMovement
{
    public float Speed { get; set; }
    public float Height { get; set; }
    public Transform Object { get; set; }

    private float sinX;
    private float speed;
    private float rotation;    

    public void Move(bool isMoving)
    {
        Rotate(isMoving);
        Hop(isMoving);
    }

    private void Hop(bool isMoving)
    {
        if (isMoving) speed = Speed;

        sinX += speed * Time.deltaTime;
        var pos = Object.localPosition;
        pos.y = Mathf.Abs(Mathf.Sin(sinX)) * Height;
        Object.localPosition = pos;

        if (sinX > Mathf.PI)
        {
            sinX = 0;
            speed = 0;
        }
    }

    private void Rotate(bool isMoving)
    {
        if (sinX == 0)
        {
            if (isMoving)
            {
                rotation = rotation <= 0 ? Random.Range(5, 15) : Random.Range(-15, 5);
                Object.localRotation = Quaternion.Euler(Vector3.forward * rotation);
            }
            else Object.localRotation = Quaternion.Euler(Vector3.forward * 0);
        }
    }
}
