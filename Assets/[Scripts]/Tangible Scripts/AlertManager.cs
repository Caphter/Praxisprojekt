using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public List<GameObject> list;

    public void ActivateSelectedAlerts()
    {
        foreach (GameObject obj in list)
        {
            if (obj.GetComponent<ShouldBeActivated>().shouldBeActivated)
            {
                obj.SetActive(true);
            }
        }
    }

    public void DeactivateAllOnReset()
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(false);
        }
    }
}
