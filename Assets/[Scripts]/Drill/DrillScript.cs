using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillScript : MonoBehaviour
{
    public SG_Grabable drill;
    public GameObject drillHead;
    public bool noEffect = false;
   

    public ChangeTrigger changeTrigger;

    //public SG_DrillBit drillBit;

    public Transform ToolAnchor;
    new AudioSource audio;


    public float rotateSpeed = 0f;
    public float maxDrillSpeed = 60.0f;

    private SG_TrackedHand hardware;

    private Vector3 startPosition = new Vector3(0, 0, 0);
    private Quaternion startRotation;

    bool pickedUp = false;
    public bool insideBox;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        rotateSpeed = (changeTrigger.AngleMap * maxDrillSpeed);
        rotateDrillHead();

        CheckGrabbed();

        putDrillInBox();

        if (rotateSpeed > 0.1 && drill.IsGrabbed())
        {
            hardware = drill.ScriptsGrabbingMe()[0].GrabScript.TrackedHand;

            if (hardware != null)
            {
                if (noEffect == true)
                {
                    hardware.SendCmd(new SGCore.Haptics.TimedThumpCmd(70, 50 / 1000.0f, -Time.deltaTime));
                }
                if (!audio.isPlaying)
                {
                    audio.Play();
                }
            }
            else
            {
                Debug.LogError("Somehow my Hardware isn't connected");
            }
        }
        else
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }
    }

    void rotateDrillHead()
    {
        drillHead.transform.Rotate(Vector3.forward, rotateSpeed);
        //drillBit.ApplyScrewTorque(rotateSpeed, rotateSpeed / maxDrillSpeed, -1);
    }



    void CheckGrabbed()
    {
        if (drill.IsGrabbed())
        {
            pickedUp = true;
            this.transform.GetComponent<Rigidbody>().useGravity = false;
            
        }

        if(!drill.IsGrabbed() && pickedUp)
        {
            //this.transform.GetComponent<Rigidbody>().useGravity = true;
           // this.transform.GetComponent<Rigidbody>().isKinematic = false;
            
        }
       
    }

    public void PlaceInsideToolbox()
    {
        insideBox = !insideBox;
    }

    public void putDrillInBox()
    {
        if (insideBox && !drill.IsGrabbed())
        {

            this.gameObject.transform.position = ToolAnchor.position;
            this.gameObject.transform.rotation = startRotation;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    

}
