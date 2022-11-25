using SGCore;
using SG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore.Haptics;
using SG.Util;

public class ChangeTrigger : MonoBehaviour
{
    public SG_Grabable grabable;
    public Transform TriggerPointA;
    public Transform TriggerPointB;

    public bool noEffect;

    [Tooltip("The minimal angle your finger needs to be in for the trigger.")]
    public float minAngle = -55;
    
    public float triggerPresure = -180;

    public float AngleMap;

    public Finger respondsTo = Finger.Index;

    float IndexAngle;

    SG_Material materialScript;

    private SG_TrackedHand hardware;
    public SG_HandDetector handDetection;

     void Start()
    {
        materialScript = GetComponent<SG_Material>();
    }

    void Update()
    {
        //check if object is being grabbed
        if (grabable.IsGrabbed())
        {
            //get the hand your holding the object with
            //List<GrabArguments> testList = grabable.ScriptsGrabbingMe();
            //SG.SG_HapticGlove hardware = grabable.GrabbedBy;

            //print(grabable.grabbedBy.Count);
            //print(grabable.grabbedBy[0].TrackedHand);

            //for (int i = 0; i < grabable.grabbedBy.Count; i++)
            //{
            //    if (grabable.grabbedBy[i].GrabScript.TrackedHand != null)
            //    {
            //        hardware = grabable.grabbedBy[i].GrabScript.TrackedHand.gloveHardware;
            //    }
            //}
            hardware = grabable.ScriptsGrabbingMe()[0].GrabScript.TrackedHand;
            //this.handle.ScriptsGrabbingMe()[0].GrabScript.TrackedHand

            print(hardware);

            //Get angle of finger
            float[] flexAngles;
            if (hardware.GetNormalizedFlexion(out flexAngles))
            {
                IndexAngle = flexAngles[1] ;
            }
            //if the function returned false, something went wrong and we shouldn't be using flexAngles.

            //map the angle
            AngleMap = Mathf.Clamp01(SG_Util.Map(IndexAngle, 0.2f, 0.5f, 0, 1));

            
            
            
            //change trigger position based on map value
            transform.position = Vector3.Lerp(TriggerPointB.position, TriggerPointA.position, AngleMap);

            int BPercantage = Mathf.RoundToInt(AngleMap * 100f);
            if (respondsTo == Finger.Index && noEffect == true)
            {
               int[] ffb = new int[5];
               ffb[(int)respondsTo] = AngleMap > 0 ? 80 : 0;
                hardware.SendCmd(new SGCore.Haptics.SG_FFBCmd(ffb));
                hardware.SendCmd(new SG_TimedBuzzCmd(Finger.Index, BPercantage, 0.020f));
                
            }
            //else
            //{
            //    int[] ffb = new int[5] { BPercantage, BPercantage, BPercantage, BPercantage, BPercantage };
            //    hardware.SendBrakeCmd(ffb);
            //}

           
           // materialScript.maxForce = Convert.ToInt32(FPercantage); ;



        }
        else
        {
            AngleMap = 0;
        }
    }
}
