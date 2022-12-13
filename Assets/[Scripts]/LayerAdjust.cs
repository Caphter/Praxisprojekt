using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerAdjust : MonoBehaviour
{
    private int layerInteractable;

    // Start is called before the first frame update
    void Start()
    {
        layerInteractable = LayerMask.NameToLayer("Interactable");

        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = layerInteractable;
        }
    }
}
