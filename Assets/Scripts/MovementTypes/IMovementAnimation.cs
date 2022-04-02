using UnityEngine;

public interface IMovementAnimation
{
    /// <summary>
    ///     Speed of the movement animation
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    ///     Height of the movement animation
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    ///     Object to animate
    /// </summary>
    public Transform Object { get; set; }

    /// <summary>
    ///     Update on the movement
    /// </summary>
    /// <param name="isMoving"> Whether or not the object is still in moving state </param>
    public void Move(bool isMoving);
}
