using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDoorViaRossAngle : MonoBehaviour
{
    private float lastImplementedAngle;
    public float currentAngle;

    /// <summary>
    /// Wenn der aktuelle �bertragene Wert des Motors nicht dem entspricht, der als letztes implementiert wurde, wird die T�r auf den aktullen Wert eingestellt
    /// </summary>
    void FixedUpdate()
    {
        if(lastImplementedAngle != currentAngle)
        {
            lastImplementedAngle = currentAngle;

            this.gameObject.transform.localRotation = Quaternion.Euler(0f,0f,currentAngle+1.435f);
        }
    }
}
