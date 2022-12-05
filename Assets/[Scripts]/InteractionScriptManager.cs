using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class InteractionScriptManager : MonoBehaviour
{
    public List<GameObject> interactableObjects;

    public void SetActiveController()
    {
        foreach(GameObject obj in interactableObjects)
        {
            obj.GetComponent<InteractionBehaviour>().enabled = false;
        }
    }

    public void SetActiveSenseGloves()
    {


    }
    
    public void SetActiveHandTracking()
    {
        foreach (GameObject obj in interactableObjects)
        {
            obj.GetComponent<InteractionBehaviour>().enabled = true;
        }
    }
}
