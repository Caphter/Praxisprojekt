using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBike : MonoBehaviour
{
    public float speed;
    public bool arrivedOnTrigger;


    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }

}
