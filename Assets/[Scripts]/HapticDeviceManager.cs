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


    - UI für 2 Wechsel Optionen -> Manuell o. Automatisch
    -> Automatisch: Wenn sie die Koordinaten eines Gerätes ändern wird eine Funktion aufgerufen, die ein weiteres Ändern checkt und nach Schwellenwertüberschreitung das Gerät wechselt
    -> Manuell: Es gibt für das gerade aktive Gerät 2 Buttons die jeweils ein- und ausgeblendet werden für die jeweils anderen Geräte
    -> UI in Reichweite platzieren, dafür einen UI-Schließ- und Aufrufbutton einbauen

    - Hand Tracking ist Baseline
    -> Individuelle Geräte-UI nach ein paar Sekunden erst einblenden, damit das Programm die Möglichkeit hat das aktive Gerät zu identifizieren.
    -> Die Hand-Modelle müssten möglichst gleich angepasst werden


    - Wenn Controller aktiv sind (in Steam VR) und wenn ihre Koordinaten mit einem gewissen Faktor geändert werden, werden Controller als haptisches Gerät ausgewählt
    - Für Sense Gloves gleiches Prinzip aber mit Trackern / Vielleicht schauen, ob es einen anderen Input gibt, der SG als aktives Gerät besser determinieren kann
    */


    private bool controllerActivated = false;
    private bool trackerActivated = false;

    private float timerReset = 0;
    public float timerTriggerAmount;
    public int offsetZeroCounterTriggerAmount;
    public UIManager uiManagerScript;

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

    private float offsetPositionTracker;
    private float offsetPositionController;

    private float thresholdTracker = 0.7f;
    private float thresholdController = 2f;

    private int counterOffsetZero = 0;

    private float timerDelayController = 0f;

    public InteractionScriptManager scriptManager;    


    // Start is called before the first frame update
    void Awake()
    {
        // Device Hierarchie Start
        senseGlovesDevice.SetActive(false);
        controllerDevice.transform.localPosition = new Vector3(15f,0f,0f);
        handTrackingDevice.SetActive(true);

        StartCoroutine(InitializePositions());        
    }

    IEnumerator InitializePositions()
    {
        yield return new WaitForSeconds(2f);

        checkActiveHapticDevice();

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
        // übergibt die aktuelle Position der Controller/Tracker
        posRightController = goRightController.transform.localPosition;
        posLeftController = goLeftController.transform.localPosition;
        posLeftTracker = goLeftTracker.transform.localPosition;
        posRightTracker = goRightTracker.transform.localPosition;


        // erhöht den Timer und ruft alle 3 Sekunden die Funktion auf
        timerReset += Time.deltaTime;
        timerDelayController += Time.deltaTime;

        if(timerReset >= timerTriggerAmount)
        {
            OffsetDecider();
            ResetPosOffsets();
        }

        OffsetUpdater();
    }

    public void OffsetUpdater()
    {
        // Checkt, ob es eine Änderung in der Positionen der Geräte gab zum letzten Frame
        if (lastPosLeftController != posLeftController || lastPosRightController != posRightController)
        {
            offsetPositionController += (float)System.Math.Round(Mathf.Abs(posLeftController.x - lastPosLeftController.x), 2) + (float)System.Math.Round(Mathf.Abs(posLeftController.y - lastPosLeftController.y), 2) + (float)System.Math.Round(Mathf.Abs(posLeftController.z - lastPosLeftController.z), 2);
            offsetPositionController += (float)System.Math.Round(Mathf.Abs(posRightController.x - lastPosRightController.x), 2) + (float)System.Math.Round(Mathf.Abs(posRightController.y - lastPosRightController.y), 2) + (float)System.Math.Round(Mathf.Abs(posRightController.z - lastPosRightController.z), 2);

            lastPosLeftController = goLeftController.transform.localPosition;
            lastPosRightController = goRightController.transform.localPosition;
        }

        if (lastPosLeftTracker != posLeftTracker || lastPosRightTracker != posRightTracker)
        {
            offsetPositionTracker += (float)System.Math.Round(Mathf.Abs(posLeftTracker.x - lastPosLeftTracker.x), 3) + (float)System.Math.Round(Mathf.Abs(posLeftTracker.y - lastPosLeftTracker.y), 3) + (float)System.Math.Round(Mathf.Abs(posLeftTracker.z - lastPosLeftTracker.z), 3);
            offsetPositionTracker += (float)System.Math.Round(Mathf.Abs(posRightTracker.x - lastPosRightTracker.x), 3) + (float)System.Math.Round(Mathf.Abs(posRightTracker.y - lastPosRightTracker.y), 3) + (float)System.Math.Round(Mathf.Abs(posRightTracker.z - lastPosRightTracker.z), 3);

            lastPosLeftTracker = goLeftTracker.transform.localPosition;
            lastPosRightTracker = goRightTracker.transform.localPosition;
        }
    }

    public void OffsetDecider()
    {
        if(offsetPositionController + offsetPositionTracker == 0)
        {
            counterOffsetZero++;

            if(counterOffsetZero == offsetZeroCounterTriggerAmount && !handTrackingDevice.activeSelf)
            {
                ActivateHandTracking();
                scriptManager.SetActiveHandTracking();
                uiManagerScript.ActivateHandTrackingUI();
                counterOffsetZero = 0;
            }
        }
        else
        {
            if(offsetPositionController >= thresholdController || offsetPositionTracker >= thresholdTracker)
            {
                if(offsetPositionController > offsetPositionTracker && controllerDevice.transform.position.x < 15 && timerDelayController > 4f)
                {
                    ActivateController();
                    scriptManager.SetActiveController();
                    uiManagerScript.ActivateControllerUI();
                    counterOffsetZero = 0;
                }
                else if(offsetPositionController < offsetPositionTracker && !senseGlovesDevice.activeSelf)
                {
                    ActivateSenseGloves();
                    uiManagerScript.ActivateSenseGlovesUI();
                    counterOffsetZero = 0;
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
        controllerDevice.transform.localPosition = new Vector3(15f, 0f, 0f);
        handTrackingDevice.SetActive(false);
        senseGlovesDevice.SetActive(true);
        Debug.Log("SG aktiviert");

        // Code für entsprechende Interaktions-Objekte zum passenden Device
    }

    public void ActivateController()
    {
        handTrackingDevice.SetActive(false);
        senseGlovesDevice.SetActive(false);
        controllerDevice.transform.localPosition = new Vector3(0f, 0f, 0f);
        Debug.Log("Controller aktiviert");

        // Code für entsprechende Interaktions-Objekte zum passenden Device
    }

    public void ActivateHandTracking()
    {
        controllerDevice.transform.localPosition = new Vector3(15f, 0f, 0f);
        senseGlovesDevice.SetActive(false);
        handTrackingDevice.SetActive(true);

        Debug.Log("Hand Tracking aktiviert");
        // Code für entsprechende Interaktions-Objekte zum passenden Device
    }

    
    public void checkActiveHapticDevice()
    {
        // initiales zurücksetzen der tracking-Variablen für neue Iteration
        controllerActivated = false;
        trackerActivated = false;

        // Checken welche Geräte in Steam VR aktiv sind
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


        // UI Anzeige, dass Devices nicht aktiviert sind
    }
    
}
