using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR;

public class HapticDeviceManager : MonoBehaviour
{
    /* 
    - Check kann beim Starten/Resetten der Szene gemacht werden oder w�hrend der Laufzeit (Wenn w�hrend der Laufzeit muss genau gemacht werden
    -> Timer einbauen der in bestimmten Abstand abfragt, ob die Wechselkriterien erf�llt werden

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
        // erh�ht den Timer und ruft alle 3 Sekunden die Funktion zum checken des aktiven haptischen Ger�tes auf
        timerCheckActiveHapticDevice += Time.deltaTime;

        if(timerCheckActiveHapticDevice >= 3f)
        {
            checkActiveHapticDevice();
        }
    }

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
}
