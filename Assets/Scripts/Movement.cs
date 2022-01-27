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
    [SerializeField, Range(0, 15)] private float movementSpeed;

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

    private IMovement hoppingMovement = new HoppingMovement();
    private IMovement rollingMovement;
    private IMovement flyingMovement;

    private IMovement currentMovement;
    #endregion


    #region Logic
    private void Awake() => entity = entity == null ? transform.GetChild(0) : entity;

    private void Start() => MovementType = movementType;

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
    #endregion

    #region Helper Methods
    private IMovement MovementTypeToIMovement(EMovementType type)
    {
        if (type == EMovementType.Hopping) return hoppingMovement;
        if (type == EMovementType.Rolling) return rollingMovement;
        return flyingMovement;
    }
    #endregion

}
