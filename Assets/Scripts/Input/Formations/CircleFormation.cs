using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFormation : IFormation
{
    public List<Vector3> GetPositionList(Vector3 targetPosition)
    {
        return GetPositionListAround(targetPosition, new float[] { .5f, 1f, 1.5f }, new int[] { 6, 12, 24 });
    }

    /// <summary>
    ///     Creates an amount of positions in the shape of a circle
    /// </summary>
    /// <param name="startPosition"> Center position </param>
    /// <param name="distance"> Distance form inner point of the circle to the positions </param>
    /// <param name="positionCount"> Amount of positions </param>
    /// <returns></returns>
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
     => Quaternion.Euler(0, 0, angle) * vec;
}
