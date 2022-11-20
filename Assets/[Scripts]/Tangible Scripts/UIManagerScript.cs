using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leap.Unity;
using Leap.Unity.Interaction;
using UHI.Tracking.InteractionEngine.Examples;

public class UIManagerScript : MonoBehaviour
{
    public GameObject uiControlPanel;
    public GameObject uiSliderPanel;
    public AlertManager alertManagerScript;
    public GameObject bike;
    public MoveBike moveBikeScript;

    public List<GameObject> alertButtonsList;
    public List<GameObject> alertShoulBeActivatedList;

    public Animator animBike;
    public Animator animRider;


    private void Start()
    {
        alertShoulBeActivatedList = alertManagerScript.list;

        foreach (GameObject obj in alertButtonsList)
        {
            if(obj.GetComponent<SimpleInteractionGlow>().correspodingShouldBeActivatedScript.shouldBeActivated == true)
            {
                obj.GetComponent<SimpleInteractionGlow>().defaultColor = Color.green;
            }
            else
            {
                obj.GetComponent<SimpleInteractionGlow>().defaultColor = Color.red;
            }
        }
    }

    public void RestartScene()
    {
        if(moveBikeScript.speed != 0f)
        {
            bike.transform.localPosition = new Vector3(0f, 0f, 0f);
            moveBikeScript.speed = 0;
            animBike.enabled = false;
            animRider.enabled = false;
        }
        else
        {
            animBike.enabled = true;
            animRider.enabled = true;
            moveBikeScript.speed = 3f;
        }
    }

    public void ControlPanelActiveControl()
    {
        if (uiControlPanel.activeSelf)
        {
            uiControlPanel.SetActive(false);
            uiSliderPanel.SetActive(false);
        }
        else
        {
            uiControlPanel.SetActive(true);
            uiSliderPanel.SetActive(true);
        }
    } 


    public void AlertListActiveControl(string alert)
    {
        foreach(GameObject obj in alertButtonsList)
        {
            if (obj.name.Contains(alert))
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
                else
                {
                    obj.SetActive(true);
                }
            }
        }
    }

}
