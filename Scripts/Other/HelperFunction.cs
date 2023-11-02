using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunction
{
    [SerializeField] private static GameObject go;
    public static GameObject Go => go;
    public static bool VectorInSameDir(Vector3 vector1, Vector3 vector2, float toleranceDegrees)
    {
        // Normalize the vectors
        vector1.Normalize();
        vector2.Normalize();

        // Calculate the dot product between the two vectors
        float dotProduct = Vector3.Dot(vector1, vector2);

        // Calculate the angle between the vectors in radians
        float angleRad = Mathf.Acos(dotProduct);

        // Convert the angle to degrees
        float angleDeg = Mathf.Rad2Deg * angleRad;

        // Check if the angle is within the specified tolerance
        return Mathf.Abs(angleDeg) <= toleranceDegrees;
    }
}
