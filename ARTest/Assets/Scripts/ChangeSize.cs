using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{

    private float scaleFactor = 0.01f;
    private Vector3 scalingVector = new Vector3(0.01f, 0.01f, 0.01f);
    
    void Update()
    {
        transform.localScale = scalingVector;
    }

    public void changeScale(float newScaleFactor)
    {
        scaleFactor = newScaleFactor;
        scalingVector.Set(newScaleFactor, newScaleFactor, newScaleFactor);
    }
}
