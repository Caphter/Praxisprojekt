using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDropped : MonoBehaviour
{
    private Vector3 originPos;
    private Quaternion originRot;
    public bool isMainObject = false;
    public bool isBuildIn = false;

    // Start is called before the first frame update
    void Start()
    {
        originPos = this.gameObject.transform.localPosition;
        originRot = this.gameObject.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UIReset()
    {
        if (isBuildIn)
        {
            isBuildIn = false;
        }

        this.gameObject.transform.localPosition = originPos;
        this.gameObject.transform.localRotation = originRot;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Deadzone" && !isBuildIn)
        {
            // setzt die Velocity auf 0, damit beim Reset nicht weiterfliegt
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (isMainObject)
            {
                this.gameObject.transform.localPosition = new Vector3(-0.877f, 0.129f, 0.293f);
                this.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if(!isMainObject && !isBuildIn)
            {
                this.gameObject.transform.localPosition = originPos;
                this.gameObject.transform.localRotation = originRot;
            }
        }
    }
}
