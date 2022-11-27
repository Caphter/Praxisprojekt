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

    private float timerCheckActiveHapticDevice = 0;

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

    // Positions-Ver�nderung Berechnungsvariablen
    //private float offsetRotationTrackerLeft;
    //private float offsetRotationTrackerRight;
    //private float offsetRotationControllerLeft;
    //private float offsetRotationControllerRight;

    public float offsetPositionTrackerLeft;
    public float offsetPositionTrackerRight;
    public float offsetPositionControllerLeft;
    public float offsetPositionControllerRight;

    private float thresholdTracker;
    private float threshholdController;

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
        posRightController = goRightController.transform.localPosition;
        posLeftController = goLeftController.transform.localPosition;
        posLeftTracker = goLeftTracker.transform.localPosition;
        posRightTracker = goRightTracker.transform.localPosition;


        // erh�ht den Timer und ruft alle 3 Sekunden die Funktion zum checken des aktiven haptischen Ger�tes auf
        timerCheckActiveHapticDevice += Time.deltaTime;

        if(timerCheckActiveHapticDevice >= 3f)
        {
            checkActiveHapticDevice();
        }

        
        // Checkt, ob es eine �nderung in der Positionen der Ger�te gab zum letzten Frame
        if(lastPosLeftController != posLeftController || lastPosRightController != posRightController)
        {
            offsetPositionControllerLeft += (float)System.Math.Round(Mathf.Abs(posLeftController.x - lastPosLeftController.x),2) + (float)System.Math.Round(Mathf.Abs(posLeftController.y - lastPosLeftController.y),2) + (float)System.Math.Round(Mathf.Abs(posLeftController.z - lastPosLeftController.z),2);
            offsetPositionControllerRight += Mathf.Abs(posRightController.x - lastPosRightController.x) + Mathf.Abs(posRightController.y - lastPosRightController.y) + Mathf.Abs(posRightController.z - lastPosRightController.z);

            lastPosLeftController = goLeftController.transform.localPosition;
            lastPosRightController = goRightController.transform.localPosition;
        }

        if (lastPosLeftTracker != posLeftTracker || lastPosRightTracker != posRightTracker)
        {
            offsetPositionTrackerLeft += Mathf.Abs(posLeftTracker.x - lastPosLeftTracker.x) + Mathf.Abs(posLeftTracker.y - lastPosLeftTracker.y) + Mathf.Abs(posLeftTracker.z - lastPosLeftTracker.z);
            offsetPositionTrackerRight += Mathf.Abs(posRightTracker.x - lastPosRightTracker.x) + Mathf.Abs(posRightTracker.y - lastPosRightTracker.y) + Mathf.Abs(posRightTracker.z - lastPosRightTracker.z);

            lastPosLeftTracker = goLeftTracker.transform.localPosition;
            lastPosRightTracker = goRightTracker.transform.localPosition;
        }

    }

    public void checkActiveHapticDevice()
    {
        // initiales zur�cksetzen der tracking-Variablen f�r neue Iteration
        timerCheckActiveHapticDevice = 0f;
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
}
