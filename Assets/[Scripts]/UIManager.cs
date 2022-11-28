using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public HapticDeviceManager deviceManageScript;

    public Text textDevice;
    public GameObject iconSenseGloves;
    public GameObject iconHandTracking;
    public GameObject iconController;

    void Update()
    {
        
    }

    public void ActivateControllerUI()
    {
        textDevice.text = "VR Controller";
        iconController.SetActive(true);
        iconHandTracking.SetActive(false);
        iconSenseGloves.SetActive(false);
    }

    public void ActivateHandTrackingUI()
    {
        textDevice.text = "Hand Tracking";
        iconController.SetActive(false);
        iconHandTracking.SetActive(true);
        iconSenseGloves.SetActive(false);
    }

    public void ActivateSenseGlovesUI()
    {
        textDevice.text = "Sense Gloves";
        iconController.SetActive(false);
        iconHandTracking.SetActive(false);
        iconSenseGloves.SetActive(true);
    }
}
