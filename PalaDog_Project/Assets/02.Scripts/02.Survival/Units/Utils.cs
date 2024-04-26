using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float DistanceToTarget(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x);
    }
}
