using UnityEngine;
using Jonko.Utils;
using System.Collections;
using System;

public enum EMovementAnimationType { Hopping, Rolling, flying }

[RequireComponent(typeof(RTSUnit))]
public class RTSMovement : MonoBehaviour
{
    #region Serialized Fields
    [Tooltip("The sprite of the object")]
    [SerializeField] private Transform entity;

    [Header("Movement Type")]
    [SerializeField] private EMovementAnimationType movementType;

    [Header("Movement Variables")]
    [SerializeField, Range(0, 0.1f)] private float destinationThresHold;
    [SerializeField, Range(0.1f, 15)] private float movementSpeed;

    [Header("Rotation Variables")]
    [SerializeField, Range(0.1f, 2)] private float timeToRotate = 1;

    [Header("Animation Variables")]
    [SerializeField] private float animationSpeed;
    [SerializeField] private float animationHeight;
    #endregion

    #region Public Fields
    public EMovementAnimationType MovementType
    {
        get { return movementType; }
        set
        {
            movementType = value;
            currentMovement = MovementTypeToIMovement(value);
            currentMovement.Object = entity;
            currentMovement.Speed = animationSpeed;
            currentMovement.Height = animationHeight;
        }
    }
    #endregion

    #region Private Fields
    private int lookingDirection = 1;

    private Vector3 spriteScale;

    private IMovementAnimation hoppingMovement = new HoppingMovementAnimation();
    private IMovementAnimation rollingMovement;
    private IMovementAnimation flyingMovement;

    private IMovementAnimation currentMovement;

    private Coroutine isMoving;
    private Coroutine isRotating;


    #endregion


    private void Start()
    {
        MovementType = movementType;
        spriteScale = transform.localScale;
    }

    private void Update() 
    {
        // Animation
        currentMovement.Move(isMoving != null);
    }


    #region Movement
    /// <summary>
    ///     Used to trigger the Move Coroutine.
    /// </summary>
    /// <param name="desiredPosition"> Vector2 position to move to. </param>
    public void MoveTo(Vector2 desiredPosition, Action actionWhenFinished = null)
    {
        if (isMoving != null) StopCoroutine(isMoving);
        isMoving = StartCoroutine(CMove(desiredPosition, actionWhenFinished));
    }

    /// <summary>
    ///     Coroutine that moves the RTS Unit to a specific location.
    /// </summary>
    private IEnumerator CMove(Vector2 desiredPosition, Action actionWhenFinished = null)
    {
        // Determine direction
        var desiredDirection = (desiredPosition - (Vector2)transform.position).normalized;

        // Check if rotation is needed
        if (Logic.OppositePositiveNegative(desiredDirection.x, lookingDirection))
            Rotate();

        // Check if position is reached and move towards that point
        while (Vector3.Distance(transform.position, desiredPosition) > destinationThresHold)
        {
            transform.position += (Vector3)desiredDirection * movementSpeed * Time.deltaTime;
            yield return null;
        }
        
        // Change variables when point is reached
        transform.position = desiredPosition;
        actionWhenFinished();
        isMoving = null;
    }
    #endregion

    #region Rotation
    /// <summary>
    ///     Used to trigger the rotation coroutine.
    /// </summary>
    public void Rotate()
    {
        if (isRotating != null) StopCoroutine(isRotating);
        isRotating = StartCoroutine(CRotate());
    }

    /// <summary>
    ///     Coroutine to make the sprite rotate over a set amount of time.
    /// </summary>
    private IEnumerator CRotate()
    {
        lookingDirection = -lookingDirection;
        // Preparing scaling variables
        var originalScale = transform.localScale;
        var desiredScale = spriteScale;
        desiredScale.x *= lookingDirection;
        // Saving starting time
        var startTime = Time.time;

        var fracComplete = 0f;
        while (fracComplete < 1)
        {
            fracComplete = (Time.time - startTime) / timeToRotate;
            transform.localScale = Vector3.Slerp(originalScale, desiredScale, fracComplete);
            yield return null;
        }

        isRotating = null;
    }
    #endregion



    #region Helper Methods
    /// <summary>
    ///     Converts the Movement animation type enum to a movement type script.
    /// </summary>
    /// <param name="type"> Type of movement </param>
    /// <returns> Movement script </returns>
    private IMovementAnimation MovementTypeToIMovement(EMovementAnimationType type)
    {
        if (type == EMovementAnimationType.Hopping) return hoppingMovement;
        if (type == EMovementAnimationType.Rolling) return rollingMovement;
        return flyingMovement;
    }
    #endregion

}
