using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using UnityEngine.UI;

public class ModeSwitchHandtracking : MonoBehaviour
{
    public InteractionSlider sliderScript;
    public UIManager uiManagerScript;

    private float currentSliderValue;
    private float lastValue;

    public Text buttonText;
    public GameObject onCanvas;
    public GameObject offCanvas;

    private void Start()
    {
        lastValue = currentSliderValue;
    }

    public void ToggleMode()
    {
        if(currentSliderValue > 0.5f)
        {
            uiManagerScript.isAutomaticMode = false;
            buttonText.text = "OFF";
            onCanvas.SetActive(true);
            offCanvas.SetActive(false);
        }
        else
        {
            uiManagerScript.isAutomaticMode = true;
            buttonText.text = "ON";
            onCanvas.SetActive(false);
            offCanvas.SetActive(true);
        }

        lastValue = currentSliderValue;
    }

    // Update is called once per frame
    void Update()
    {
        currentSliderValue = sliderScript.HorizontalSliderPercent;

        if(currentSliderValue != lastValue)
        {
            ToggleMode();
        }
    }
}
