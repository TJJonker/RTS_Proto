using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField, Range(0, 0.1f)] private float destinationThresHold;
    [SerializeField, Range(0, 15)] private float movementSpeed;

    private Coroutine Movement;

    public void MoveObject(Vector2 desiredPosition)
    {
        if(Movement != null) StopCoroutine(Movement);
        Movement = StartCoroutine(Moving(desiredPosition));
    }

    private IEnumerator Moving(Vector2 desiredPosition)
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
    }
}
