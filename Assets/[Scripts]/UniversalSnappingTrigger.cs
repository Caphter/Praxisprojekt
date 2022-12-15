using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap.Unity.Interaction;
using Valve.VR.InteractionSystem;
using Valve.VR;


public class UniversalSnappingTrigger : MonoBehaviour
{
    static public int counterStifte = 0;
    static public int counterFedern = 0;

    public GameObject baseObject;
    public AssemblyManager assemblyManagerScript;

    public GameObject targetPreviewObject;
    public float rotationSpeed;
    public float positionSpeed;
    private GameObject collidedObject;

    public GameObject Controller1;
    public GameObject Controller2;

    public GameObject postSnapObject;
    


    // Update is called once per frame
    void Update()
    {

    }

    public void TransformPosition()
    {
        collidedObject.transform.localPosition = targetPreviewObject.transform.localPosition;
        collidedObject.transform.localRotation = targetPreviewObject.transform.localRotation;

        //collidedObject.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        //collidedObject.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPosition, positionSpeed * Time.deltaTime)
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CurrentActiveObject")
        {
            // Starte die Positions-Transformation
            collidedObject = other.gameObject;

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
            catch (Exception)
            {
                Debug.Log("das kollidierte Objekt hat das Dropped-Skript nicht!");
            }

            // Skripte werden ausgeschaltet
            Controller1.GetComponent<Valve.VR.InteractionSystem.Hand>().DetachObject(collidedObject);
            Controller2.GetComponent<Valve.VR.InteractionSystem.Hand>().DetachObject(collidedObject);

            collidedObject.GetComponent<Collider>().enabled = false;
            collidedObject.GetComponent<InteractionBehaviour>().enabled = false;
            collidedObject.GetComponent<Rigidbody>().isKinematic = true;

            collidedObject.SetActive(false);
            postSnapObject.SetActive(true);

            /*
            

            // Setzt das Objekt als Kind von dem Base-Objekt
            other.gameObject.transform.SetParent(baseObject.transform);



            TransformPosition();
            */

            // triggert die Funktion für den nächsten Bauschritt
            assemblyManagerScript.NextStepDecider();

            // Tag wird zurückgesetzt
            other.tag = "NotActiveObject";

            // deaktiviert das Preview Model des getriggerten Colliders
            targetPreviewObject.SetActive(false);

            // deaktiviert den aktuellen Trigger
            this.gameObject.SetActive(false);

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
