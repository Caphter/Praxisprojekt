using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeTriggerInit : MonoBehaviour
{
    public Animator anim_bike;
    public Animator anim_rider;

    public string modus;

    void Start()
    {
        anim_rider.SetTrigger(modus);
        anim_bike.SetTrigger(modus);

    }



}
