using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNPC : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
    }
}
