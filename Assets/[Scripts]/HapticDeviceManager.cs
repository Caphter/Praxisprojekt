using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR;

public class HapticDeviceManager : MonoBehaviour
{
    /* 
    - Check kann beim Starten/Resetten der Szene gemacht werden oder während der Laufzeit (Wenn während der Laufzeit muss genau gemacht werden
    -> Timer einbauen der in bestimmten Abstand abfragt, ob die Wechselkriterien erfüllt werden

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
    public GameObject senseGlovesDevice;
    public GameObject controllerDevice;
    public GameObject handTrackingDevice;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // erhöht den Timer und ruft alle 3 Sekunden die Funktion zum checken des aktiven haptischen Gerätes auf
        timerCheckActiveHapticDevice += Time.deltaTime;

        if(timerCheckActiveHapticDevice >= 3f)
        {
            checkActiveHapticDevice();
        }
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

    }
}
