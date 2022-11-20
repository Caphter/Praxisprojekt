using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeTrigger : MonoBehaviour
{
    public MoveBike moveBikeScript;
    public AlertManager alertManagerScript;
    public BikeTriggerInit bikeTriggerScript;
    public TurnDoorViaRossAngle currAng;

    private void OnTriggerStay(Collider other)
    {   
        if(other.tag == "Bike" && currAng.currentAngle > 5f)
        {
            moveBikeScript.arrivedOnTrigger = true;
            alertManagerScript.ActivateSelectedAlerts();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Bike")
        {
            moveBikeScript.arrivedOnTrigger = false;
            alertManagerScript.DeactivateAllOnReset();
        }
    }
}
