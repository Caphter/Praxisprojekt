using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UniversalSnappingTrigger : MonoBehaviour
{
    public int counterStifte = 0;
    public int counterFedern = 0;

    public GameObject baseObject;
    public AssemblyManager assemblyManagerScript;

    public Vector3 targetPosition;
    public Quaternion targetRotation;
    public float rotationSpeed;
    public float positionSpeed;
    private bool triggered = false;
    private GameObject collidedObject;


    // Update is called once per frame
    void Update()
    {
        // Bewegt das Object zu der gezielten Position
        if (triggered)
        {
            collidedObject.transform.localPosition = targetPosition;
            collidedObject.transform.localRotation = targetRotation;

            //collidedObject.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            //collidedObject.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPosition, positionSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CurrentActiveObject")
        {
            // Zählt wie viele Stifte oder Federn schon verbaut sind
            if (other.gameObject.name.Contains("Feder"))
            {
                counterFedern++;
            }
            else if (other.gameObject.name.Contains("Stift"))
            {
                counterStifte++;
            }

            // Sagt, dass das Objekt jetzt verbaut ist --> kein Zurücksetzen beim Runterfallen mehr
            try
            {
                other.gameObject.GetComponent<ObjectDropped>().isBuildIn = true;
            }
            catch (Exception e)
            {
                Debug.Log("das kollidierte Objekt hat das Dropped-Skript nicht!");
            }

            // Setzt das Objekt als Kind von dem Base-Objekt
            other.gameObject.transform.SetParent(baseObject.transform);

            // triggert die Funktion für den nächsten Bauschritt
            assemblyManagerScript.NextStepDecider();

            // Starte die Positions-Transformation
            triggered = true;
            collidedObject = other.gameObject;

            // Skripte werden ausgeschaltet???

            // Tag wird zurückgesetzt
            other.tag = "NotActiveObject";
        }
        else if(other.tag == "NotActiveObject")
        {
            assemblyManagerScript.PreviewsRed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "NotActiveObject")
        {
            assemblyManagerScript.PreviewsGreen();
        }
    }
}
