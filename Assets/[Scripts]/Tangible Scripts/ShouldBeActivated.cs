using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouldBeActivated : MonoBehaviour
{
    public bool shouldBeActivated = false;

    public void ToggleShouldBeActivated()
    {
        if (shouldBeActivated)
        {
            shouldBeActivated = false;
        }
        else
        {
            shouldBeActivated = true;
        }
    }

}
