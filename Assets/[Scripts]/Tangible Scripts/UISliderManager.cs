using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using RosSharp.RosBridgeClient;
using UnityEngine.UI;

public class UISliderManager : MonoBehaviour
{
    public float sliderValueForce;
    public float sliderValueAngle;
    //public float silderValueForceConverted;
    public float sliderValueAngleConverted;
    public float forceInLockingAngle;
    public InteractionSlider interactionSliderScriptForce;
    public InteractionSlider interactionSliderScriptLockingAngle;

    public GameObject textSliderLockingAngle;
    private Text textComponentLockingAngle;
    public GameObject textSliderForce;
    private Text textComponentForce;

    public float LastCurrentAngle;

    //Testing Variables
    public bool lockedOnAngle = false;
    public bool forceInLockedAngle = false;
    public float forceValueAfterLockingAngle;

    //Ros Scripts
    public TurnDoorViaRossAngle currentAngleScript;
    public PublisherDoorForce publisherDoorForce;
    public PublisherOverlayPositionRel publisherOverlayPositionRel;


    private void Start()
    {
        LastCurrentAngle = currentAngleScript.currentAngle;

        textComponentLockingAngle = textSliderLockingAngle.GetComponent<Text>();
        textComponentForce = textSliderForce.GetComponent<Text>();

        LockingAngleConversion();
        textComponentLockingAngle.text = "LOCKING ANGLE:  " + System.Math.Round(sliderValueAngleConverted, 0) + "°";
    }

    // Update is called once per frame
    void Update()
    {
        // checkt LockingAngle Slider Veränderungen
        if (sliderValueAngle != interactionSliderScriptLockingAngle.HorizontalSliderPercent)
        {
            sliderValueAngle = interactionSliderScriptLockingAngle.HorizontalSliderPercent;

            // Umwandlung des 0-1 Dez. Wert in entsprechenden Winkel
            LockingAngleConversion();

            // anpassen des UI Texts
            textComponentLockingAngle.text = "LOCKING ANGLE:  " + System.Math.Round(sliderValueAngleConverted, 0) + "°";
        }

        // checkt Force Slider Veränderungen
        if (sliderValueForce != interactionSliderScriptForce.HorizontalSliderPercent)
        {
            sliderValueForce = interactionSliderScriptForce.HorizontalSliderPercent;

            // anpassen des UI Texts
            textComponentForce.text = "FORCE:  " + System.Math.Round(sliderValueForce * 100f, 0) + "%";
        }

        // Checkt ob die Tür sich dreht und führt die entsprechenden Force-Änderungen aus
        if(currentAngleScript.currentAngle != LastCurrentAngle)
        {
            LastCurrentAngle = currentAngleScript.currentAngle;


            // Checkt, ob der aktuelle Winkler in der Nähe des Feststellwinkels liegt um entsprechend Force anzupassen
            if ((currentAngleScript.currentAngle >= sliderValueAngleConverted - 5f && currentAngleScript.currentAngle <= sliderValueAngleConverted + 5f) || currentAngleScript.currentAngle >= 80f)
            {
                forceInLockedAngle = true;
                publisherDoorForce.positionOverlayFactor = forceInLockingAngle;
            }
            else if(currentAngleScript.currentAngle >= sliderValueAngleConverted + 5f && currentAngleScript.currentAngle < 80f)
            {
                publisherDoorForce.positionOverlayFactor = forceValueAfterLockingAngle;
            }
            else
            {
                forceInLockedAngle = false;
                publisherDoorForce.positionOverlayFactor = sliderValueForce * 0.5f;
            }


            // Setzt den Locking Point auf 85° bei kompletter auslenkung, darunter auf den slider-Wert und darunter auf 0
            if(currentAngleScript.currentAngle >= 80f)
            {
                publisherOverlayPositionRel.OverlayPositionRel = 85f * 540;
            }
            else if(currentAngleScript.currentAngle < 80f && currentAngleScript.currentAngle > sliderValueAngleConverted)
            {
                lockedOnAngle = true;

                publisherOverlayPositionRel.OverlayPositionRel = sliderValueAngle * 540;
            }
            else if(currentAngleScript.currentAngle < (sliderValueAngleConverted - 5f))
            {
                lockedOnAngle = false;

                publisherOverlayPositionRel.OverlayPositionRel = 0;
            }
        }
    }

    public void LockingAngleConversion()
    {
        sliderValueAngleConverted = sliderValueAngle * 50f + 20;
    }
}
