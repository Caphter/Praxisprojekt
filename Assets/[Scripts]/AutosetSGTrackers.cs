using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR;

public class AutosetSGTrackers : MonoBehaviour
{
    public SteamVR_TrackedObject trackerScriptRight;
    public SteamVR_TrackedObject trackerScriptLeft;

    bool rightAssigned = false;
    int indexOfFirstAssigned = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Setzt das Device auf 16
        trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device1;

        ETrackedPropertyError error = new ETrackedPropertyError();

        for (int i = 0; i < 15; i++)
        {
            var id = new System.Text.StringBuilder(64);
            OpenVR.System.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, id, 64, ref error);

            if (id.ToString().Contains("vr_tracker"))
            {
                AssignCorrespondingTrackerToIndex(i);
            }

            Debug.Log("test id " + i + ": " + id);
        }
    }

    public void AssignCorrespondingTrackerToIndex(int index){

        // Setzt den Verweis auf den Index, der dem ersten Tracker zugewiesen wurde
        if(indexOfFirstAssigned == 0)
        {
            indexOfFirstAssigned = index;
        }

        if (!rightAssigned)
        {
            rightAssigned = true;

            switch (index){
                case 1:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device1;
                    break;
                case 2:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device2;
                    break;
                case 3:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device3;
                    break;
                case 4:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device4;
                    break;
                case 5:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device5;
                    break;
                case 6:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device6;
                    break;
                case 7:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device7;
                    break;
                case 8:
                    trackerScriptRight.index = SteamVR_TrackedObject.EIndex.Device8;
                    break;
            }
        }
        else if(rightAssigned && index != indexOfFirstAssigned)
        {
            switch (index)
            {
                case 1:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device1;
                    break;
                case 2:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device2;
                    break;
                case 3:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device3;
                    break;
                case 4:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device4;
                    break;
                case 5:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device5;
                    break;
                case 6:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device6;
                    break;
                case 7:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device7;
                    break;
                case 8:
                    trackerScriptLeft.index = SteamVR_TrackedObject.EIndex.Device8;
                    break;
            }
        }
    }
}
