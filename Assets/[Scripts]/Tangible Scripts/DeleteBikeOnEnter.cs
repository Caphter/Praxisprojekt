using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBikeOnEnter : MonoBehaviour
{
    public GameObject bike;
    public MoveBike moveBikeScript;
    public UIManagerScript uiManagerScript;

    // Setzt Fahrrad zurück auf Startposition und setzt den Speed auf 0
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bike")
        {
            uiManagerScript.RestartScene();
        }
    }
}
