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

    // Positions-Veränderung Berechnungsvariablen
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


        // erhöht den Timer und ruft alle 3 Sekunden die Funktion zum checken des aktiven haptischen Gerätes auf
        timerCheckActiveHapticDevice += Time.deltaTime;

        if(timerCheckActiveHapticDevice >= 3f)
        {
            checkActiveHapticDevice();
        }

        
        // Checkt, ob es eine Änderung in der Positionen der Geräte gab zum letzten Frame
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
        // initiales zurücksetzen der tracking-Variablen für neue Iteration
        timerCheckActiveHapticDevice = 0f;
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

    }
}
