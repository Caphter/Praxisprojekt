using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheibenschraubeScript : MonoBehaviour
{
    private bool isScrewedIn = false;
    public static int screwedCount = 0;

    public GameObject correspondingArrow;
    public GameObject correspondingCheckmark;

    IEnumerator TimeCheckmark()
    {
        correspondingCheckmark.SetActive(true);
        yield return new WaitForSeconds(2);
        correspondingCheckmark.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Drill")
        {
            isScrewedIn = true;
            screwedCount++;
            correspondingArrow.SetActive(false);
            StartCoroutine(TimeCheckmark());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Drill" && !isScrewedIn)
        {
            correspondingArrow.SetActive(true);
        }
    }
}
