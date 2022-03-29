using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAction : MonoBehaviour
{
    private float zBoundary = 2.2f;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        ApplyTransformBoundaries();
    }

    private void ApplyTransformBoundaries()
    {
        if (transform.localPosition.z < 0)
        {
            transform.localPosition = Vector3.zero;
        }
        else if (transform.localPosition.z > zBoundary)
        {
            transform.localPosition = new Vector3(0, 0, zBoundary);
        }
    }
}
