using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivateHands : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject lefthand;

    // Start is called before the first frame update
    void Update()
    {
        if (!rightHand.activeSelf)
        {
            rightHand.SetActive(true);
        }
        if (!lefthand.activeSelf)
        {
            lefthand.SetActive(true);
        }
    }
}
