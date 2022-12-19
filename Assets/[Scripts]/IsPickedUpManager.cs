using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPickedUpManager : MonoBehaviour
{
    private CheckIfPickedUp checkingScript;

    public GameObject correspondingArrow;
    public List<GameObject> correspondingStandColliders;

    private int LayerNormal;
    private int LayerMoving;


    // Start is called before the first frame update
    void Start()
    {
        LayerNormal = LayerMask.NameToLayer("Interactable");
        LayerMoving = LayerMask.NameToLayer("InteractableMoving");
        checkingScript = this.gameObject.GetComponent<CheckIfPickedUp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkingScript.objectMoving)
        {
            //this.gameObject.layer = LayerMoving;
            correspondingArrow.SetActive(false);

            if(correspondingStandColliders != null)
            {
                foreach (GameObject obj in correspondingStandColliders)
                {
                    //obj.GetComponent<Collider>().enabled = false;
                }
            }
        }
        else if(!checkingScript.objectMoving && this.gameObject.tag == "CurrentActiveObject")
        {
            //this.gameObject.layer = LayerNormal;
            correspondingArrow.SetActive(true);

            if (correspondingStandColliders != null)
            {
                foreach (GameObject obj in correspondingStandColliders)
                {
                    //obj.GetComponent<Collider>().enabled = true;
                }
            }
        }


    }
}
