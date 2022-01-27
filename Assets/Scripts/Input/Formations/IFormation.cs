using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFormation
{
    public List<Vector3> GetPositionList(Vector3 targetPosition);
}
