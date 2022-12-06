using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPickedUp : MonoBehaviour
{
    public bool isPickedUp = false;

    public Transform currentObjTrans;
    public Transform trans1;
    public Transform trans2;
    public Transform trans3;
    public Transform Hand1;

    private float intervallTimer = 0f;
    private float cycleTimer = 0f;
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        trans1 = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentObjTrans = this.gameObject.transform;

        if (trans1 != this.gameObject.transform)
        {
            triggered = true;
        }

        if (triggered)
        {
            intervallTimer += Time.deltaTime;
            cycleTimer += Time.deltaTime;
        }

        if(intervallTimer >= 1 / 6f)
        {
            intervallTimer = 0;

        }
    }
}
