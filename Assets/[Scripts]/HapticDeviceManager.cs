using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR;

public class HapticDeviceManager : MonoBehaviour
{
    /* 
    - HIERARCHIE: Controller > Sense Gloves > Hand Tracking
    -> Fall einbauen, dass Controller mit Sense-Gloves benutzt werden


    - UI f�r 2 Wechsel Optionen -> Manuell o. Automatisch
    -> Automatisch: Wenn sie die Koordinaten eines Ger�tes �ndern wird eine Funktion aufgerufen, die ein weiteres �ndern checkt und nach Schwellenwert�berschreitung das Ger�t wechselt
    -> Manuell: Es gibt f�r das gerade aktive Ger�t 2 Buttons die jeweils ein- und ausgeblendet werden f�r die jeweils anderen Ger�te
    -> UI in Reichweite platzieren, daf�r einen UI-Schlie�- und Aufrufbutton einbauen

    - Hand Tracking ist Baseline
    -> Individuelle Ger�te-UI nach ein paar Sekunden erst einblenden, damit das Programm die M�glichkeit hat das aktive Ger�t zu identifizieren.
    -> Die Hand-Modelle m�ssten m�glichst gleich angepasst werden


    - Wenn Controller aktiv sind (in Steam VR) und wenn ihre Koordinaten mit einem gewissen Faktor ge�ndert werden, werden Controller als haptisches Ger�t ausgew�hlt
    - F�r Sense Gloves gleiches Prinzip aber mit Trackern / Vielleicht schauen, ob es einen anderen Input gibt, der SG als aktives Ger�t besser determinieren kann
    */


    private bool controllerActivated = false;
    private bool trackerActivated = false;

    private float timerReset = 0;
    public float timerTriggerAmount;
    public int offsetZeroCounterTriggerAmount;

    // Devices
    [Header("Haptic Devices:")]
    public GameObject senseGlovesDevice;
    public GameObject controllerDevice;
    public GameObject handTrackingDevice;

    // Zu trackende Positionen
    [Header("Device Positions:")]
    public GameObject goRightController;
    public GameObject goLeftController;
    public GameObject goRightTracker;
    public GameObject goLeftTracker;

    private Vector3 posRightController;
    private Vector3 posLeftController;
    private Vector3 posRightTracker;
    private Vector3 posLeftTracker;

    private Vector3 lastPosRightController;
    private Vector3 lastPosLeftController;
    private Vector3 lastPosRightTracker;
    private Vector3 lastPosLeftTracker;

    public float offsetPositionTracker;
    public float offsetPositionController;

    private float thresholdTracker;
    private float thresholdController;

    
    private int counterOffsetZero = 0;


    // Start is called before the first frame update
    void Awake()
    {
        // Device Hierarchie Start
        //senseGlovesDevice.SetActive(false);
        //controllerDevice.SetActive(false);
        //handTrackingDevice.SetActive(false);

        StartCoroutine(InitializePositions());

        Debug.Log("Zuweisung");
        
    }

    IEnumerator InitializePositions()
    {
        yield return new WaitForSeconds(2f);

        posRightController = goRightController.transform.localPosition;
        posLeftController = goLeftController.transform.localPosition;
        posLeftTracker = goLeftTracker.transform.localPosition;
        posRightTracker = goRightTracker.transform.localPosition;

        lastPosLeftController = posLeftController;
        lastPosLeftTracker = posLeftTracker;
        lastPosRightController = posRightController;
        lastPosRightTracker = posRightTracker;
    }

    // Update is called once per frame
    void Update()
    {
        // �bergibt die aktuelle Position der Controller/Tracker
        posRightController = goRightController.transform.localPosition;
        posLeftController = goLeftController.transform.localPosition;
        posLeftTracker = goLeftTracker.transform.localPosition;
        posRightTracker = goRightTracker.transform.localPosition;


        // erh�ht den Timer und ruft alle 3 Sekunden die Funktion auf
        timerReset += Time.deltaTime;

        if(timerReset >= timerTriggerAmount)
        {
            OffsetDecider();
            ResetPosOffsets();
        }

        OffsetUpdater();
    }

    public void OffsetUpdater()
    {
        // Checkt, ob es eine �nderung in der Positionen der Ger�te gab zum letzten Frame
        if (lastPosLeftController != posLeftController || lastPosRightController != posRightController)
        {
            offsetPositionController += (float)System.Math.Round(Mathf.Abs(posLeftController.x - lastPosLeftController.x), 2) + (float)System.Math.Round(Mathf.Abs(posLeftController.y - lastPosLeftController.y), 2) + (float)System.Math.Round(Mathf.Abs(posLeftController.z - lastPosLeftController.z), 2);
            offsetPositionController += (float)System.Math.Round(Mathf.Abs(posRightController.x - lastPosRightController.x), 2) + (float)System.Math.Round(Mathf.Abs(posRightController.y - lastPosRightController.y), 2) + (float)System.Math.Round(Mathf.Abs(posRightController.z - lastPosRightController.z), 2);

            lastPosLeftController = goLeftController.transform.localPosition;
            lastPosRightController = goRightController.transform.localPosition;
        }

        if (lastPosLeftTracker != posLeftTracker || lastPosRightTracker != posRightTracker)
        {
            offsetPositionTracker += (float)System.Math.Round(Mathf.Abs(posLeftTracker.x - lastPosLeftTracker.x), 2) + (float)System.Math.Round(Mathf.Abs(posLeftTracker.y - lastPosLeftTracker.y), 2) + (float)System.Math.Round(Mathf.Abs(posLeftTracker.z - lastPosLeftTracker.z), 2);
            offsetPositionTracker += (float)System.Math.Round(Mathf.Abs(posRightTracker.x - lastPosRightTracker.x), 2) + (float)System.Math.Round(Mathf.Abs(posRightTracker.y - lastPosRightTracker.y), 2) + (float)System.Math.Round(Mathf.Abs(posRightTracker.z - lastPosRightTracker.z), 2);

            lastPosLeftTracker = goLeftTracker.transform.localPosition;
            lastPosRightTracker = goRightTracker.transform.localPosition;
        }
    }

    public void OffsetDecider()
    {
        if(offsetPositionController + offsetPositionTracker == 0)
        {
            counterOffsetZero++;

            if(counterOffsetZero == offsetZeroCounterTriggerAmount && !senseGlovesDevice.activeSelf)
            {
                ActivateHandTracking();
                counterOffsetZero = 0;
            }
        }
        else
        {
            if(offsetPositionController >= thresholdController || offsetPositionTracker >= thresholdTracker)
            {
                if(offsetPositionController > offsetPositionTracker)
                {
                    ActivateController();
                }
                else
                {
                    ActivateSenseGloves();
                }
            }
        }

    }

    public void ResetPosOffsets()
    {
        timerReset = 0f;

        offsetPositionController = 0f;
        offsetPositionTracker = 0f;
    }

    public void ActivateSenseGloves()
    {
        //controllerDevice.SetActive(false);
        //handTrackingDevice.SetActive(false);
        //senseGlovesDevice.SetActive(true);
        Debug.Log("SG activiert");

        // Code f�r entsprechende Interaktions-Objekte zum passenden Device
    }

    public void ActivateController()
    {
        //handTrackingDevice.SetActive(false);
        //senseGlovesDevice.SetActive(false);
        //controllerDevice.SetActive(true);
        Debug.Log("Controller activiert");

        // Code f�r entsprechende Interaktions-Objekte zum passenden Device
    }

    public void ActivateHandTracking()
    {
        //controllerDevice.SetActive(false);
        //senseGlovesDevice.SetActive(false);
        //handTrackingDevice.SetActive(true);

        Debug.Log("Hand Tracking activiert");
        // Code f�r entsprechende Interaktions-Objekte zum passenden Device
    }

    /*
    public void checkActiveHapticDevice()
    {
        // initiales zur�cksetzen der tracking-Variablen f�r neue Iteration
        controllerActivated = false;
        trackerActivated = false;

        // Checken welche Ger�te in Steam VR aktiv sind
        ETrackedPropertyError error = new ETrackedPropertyError();

        for (int i = 0; i < 6; i++)
        {
            var id = new System.Text.StringBuilder(64);
            OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, id, 64, ref error);

            if (id.ToString().Contains("vr_tracker"))
            {
                controllerActivated = true;
            }

            if (id.ToString().Contains("vr_controller"))
            {
                trackerActivated = true;
            }
        }

    }
    */
}