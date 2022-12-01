using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    //public KeyCode keyPressed = KeyCode.Space;
    private Vector3 startPosition = new Vector3(0, 0, 0);
    public float jumpingRange = 1;
    public float jumpingSpeed = 1;
    private bool startJumping = true;
    private bool goingDown = true;
    private float travelledDistance = 0f;

    private void Awake()
    {
        startPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // test to see if the range and speed are ok
        //if (Input.GetKeyDown(keyPressed))
        //{
        //    this.gameObject.SetActive(true);
        //    ResetArrow();
        //    startJumping = !startJumping;
        //    print("lets start");
        //}

        if (startJumping)
        {
            if (goingDown)
            {
                if (travelledDistance <= jumpingRange)
                {
                    this.transform.Translate(Vector3.up * jumpingSpeed * Time.deltaTime);
                    travelledDistance = travelledDistance + (jumpingSpeed * Time.deltaTime);
                }
                else
                {
                    goingDown = false;
                }
            }
            else
            {
                if (travelledDistance >= 0)
                {
                    this.transform.Translate(Vector3.down * jumpingSpeed * Time.deltaTime);
                    travelledDistance = travelledDistance - (jumpingSpeed * Time.deltaTime);
                }
                else
                {
                    goingDown = true;
                }
            }
        }
    }

    public void StartArrow()
    {
        ResetArrow();
        this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        startJumping = !startJumping;       
    }

    public void EndArrow()
    {
        startJumping = !startJumping;
        this.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    public void ResetArrow()
    {
        this.transform.localPosition = startPosition;
        goingDown = true;
        travelledDistance = 0;       
    }

}
