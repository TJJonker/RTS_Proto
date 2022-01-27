using System.Collections;
using UnityEngine;

public enum EMovementType { Hopping, Rolling, flying }

public class Movement : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Transform entity;

    [Header("Movement Type")]
    [SerializeField] private EMovementType movementType;

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
    public EMovementType MovementType
    { 
        get { return movementType; }
        set { 
            movementType = value; 
            currentMovement = MovementTypeToIMovement(value);
            currentMovement.Object = entity;
            currentMovement.Speed = animationSpeed;
            currentMovement.Height = animationHeight;
        }
    }
    #endregion

    #region Private Fields
    private Coroutine Moving;
    private Coroutine Rotating;

    private int lookingDirection = 1;

    private Vector3 spriteScale;

    private IMovement hoppingMovement = new HoppingMovement();
    private IMovement rollingMovement;
    private IMovement flyingMovement;

    private IMovement currentMovement;
    #endregion


    #region Logic
    private void Awake() => entity = entity == null ? transform.GetChild(0) : entity;

    private void Start()
    {
        MovementType = movementType;
        spriteScale = transform.localScale;
    }

    private void Update() => currentMovement.Move(Moving != null);



    public void MoveObject(Vector2 desiredPosition)
    {
        if (Moving != null) StopCoroutine(Moving);
        Moving = StartCoroutine(Move(desiredPosition));
    }

    private IEnumerator Move(Vector2 desiredPosition)
    {
        // Calculating direction to move in
        var direction = desiredPosition - (Vector2)transform.position;
        direction.Normalize();

        // Rotate object if needed
        RotateObject(direction.x);

        // Moving
        while (Vector3.Distance(transform.position, desiredPosition) > destinationThresHold)
        {
            transform.position += (Vector3)direction * movementSpeed * Time.deltaTime;
            yield return null;
        }
        // Setting position
        transform.position = desiredPosition;
        Moving = null;
    }

    private void RotateObject(float direction)
    {
        if (!OppositePositiveNegative(direction, lookingDirection)) return;
        if (Rotating != null) StopCoroutine(Rotating);
        Rotating = StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        // Rotate lookingDirection
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

        Rotating = null;
    }
    #endregion

    #region Helper Methods
    private IMovement MovementTypeToIMovement(EMovementType type)
    {
        if (type == EMovementType.Hopping) return hoppingMovement;
        if (type == EMovementType.Rolling) return rollingMovement;
        return flyingMovement;
    }

    private bool OppositePositiveNegative(float i1, float i2)
    {
        return i1 > 0 && i2 < 0 || i1 < 0 && i2 > 0;
    }
    #endregion

}
