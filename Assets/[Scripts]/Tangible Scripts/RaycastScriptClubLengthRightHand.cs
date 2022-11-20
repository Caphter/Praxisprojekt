using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScriptClubLengthRightHand : MonoBehaviour
{
    [SerializeField] private LayerMask courseLayer;

    public GameObject handPositionObject;
    public float raycastLength;
    RaycastHit hit;
    public float maxStabLength;
    public float maxClubLength;

    void Update()
    {

        Debug.DrawRay(handPositionObject.transform.position, handPositionObject.transform.forward * maxStabLength, Color.yellow);
        Debug.DrawRay(handPositionObject.transform.position, handPositionObject.transform.forward * maxClubLength, Color.red);


        if (Physics.Raycast(handPositionObject.transform.position, handPositionObject.transform.forward, out hit, maxClubLength, courseLayer))
        {
            raycastLength = hit.distance;
        }
        else
        {
            raycastLength = maxStabLength;
        }
    }
}
