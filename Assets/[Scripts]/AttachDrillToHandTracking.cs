using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachDrillToHandTracking : MonoBehaviour
{
    public GameObject rightHandParent;
    public GameObject leftHandParent;
    public GameObject detachParent;

    public GameObject targetTransform;

    public bool triggered = false;


    private void Update()
    {
        if (triggered)
        {
            this.gameObject.transform.localPosition = targetTransform.transform.localPosition;
            this.gameObject.transform.localRotation = targetTransform.transform.localRotation;
        }
    }
    public void AttachDrillToHand()
    {
        triggered = true;
        //this.gameObject.transform.SetParent(rightHandParent.transform);


    }

    public void DetachDrillFromHand()
    {
        triggered = false;
        //this.gameObject.transform.SetParent(detachParent.transform);
    }
}
