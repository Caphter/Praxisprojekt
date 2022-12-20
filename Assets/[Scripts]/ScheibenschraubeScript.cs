using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheibenschraubeScript : MonoBehaviour
{
    private bool isScrewedIn = false;
    private bool finishExecuted = false;
    public static int screwedCount = 0;

    public GameObject arrowAkkuschrauber;
    public GameObject correspondingArrow;
    public GameObject correspondingCheckmark;
    public GameObject finishCanvas;

    IEnumerator TimeCheckmark()
    {
        correspondingCheckmark.SetActive(true);
        yield return new WaitForSeconds(2);
        correspondingCheckmark.SetActive(false);
    }

    IEnumerator AssemblyFinished()
    {
        arrowAkkuschrauber.SetActive(false);
        yield return new WaitForSeconds(1);
        finishCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        finishCanvas.SetActive(false);
        finishExecuted = true;
    }

    // Enter auf Stay umschreiben, wenn ich wirklich den trigger implementiert habe
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Drill" && !isScrewedIn)
        {
            isScrewedIn = true;
            screwedCount++;
            correspondingArrow.SetActive(false);
            StartCoroutine(TimeCheckmark());
        }

        if(screwedCount >= 9 && !finishExecuted)
        {
            finishExecuted = true;
            StartCoroutine(AssemblyFinished());
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
