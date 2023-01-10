using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public HapticDeviceManager deviceManageScript;

    public Text textDevice;
    public GameObject iconSenseGloves;
    public GameObject iconHandTracking;
    public GameObject iconController;

    public bool isAutomaticMode = true;

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActivateControllerUI()
    {
        textDevice.text = "VR Controller";
        iconController.SetActive(true);
        iconHandTracking.SetActive(false);
        iconSenseGloves.SetActive(false);

        deviceManageScript.ActivateController();
    }

    public void ActivateHandTrackingUI()
    {
        textDevice.text = "Hand Tracking";
        iconController.SetActive(false);
        iconHandTracking.SetActive(true);
        iconSenseGloves.SetActive(false);

        deviceManageScript.ActivateHandTracking();
    }

    public void ActivateSenseGlovesUI()
    {
        textDevice.text = "Sense Gloves";
        iconController.SetActive(false);
        iconHandTracking.SetActive(false);
        iconSenseGloves.SetActive(true);

        deviceManageScript.ActivateSenseGloves();
    }
}
