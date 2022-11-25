using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class Autoassign_SG_Trackers : MonoBehaviour
{
    public SG_HapticGlove rightGloveScript;
    public SG_HapticGlove leftGloveScript;

    public Transform leftTracker;
    public Transform rightTracker;


    /// <summary>
    /// Falls die Initalisierung der Tracker-Transforms auf den Handschuhen nicht funktioniert, wird durch die Update funktion manuell ein erneutes Zuweisen durchgeführt
    /// </summary>
    void Update()
    {
        if (rightGloveScript.wristTrackingObj == null || leftGloveScript.wristTrackingObj == null)
        {
            if (rightGloveScript.wristTrackingObj == null)
            {
                rightGloveScript.wristTrackingObj = rightTracker;
                //SGCore.PosTrackingHardware wristTrackingOffsets = SGCore.PosTrackingHardware.ViveTracker;
            }

            if (leftGloveScript.wristTrackingObj == null)
            {
                leftGloveScript.wristTrackingObj = leftTracker;
                //SGCore.PosTrackingHardware wristTrackingOffsets = SGCore.PosTrackingHardware.ViveTracker;
            }

        }
    }
}
