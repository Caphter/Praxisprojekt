using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyManager : MonoBehaviour
{
    /* 
    - Teile bekommen BASE als Parent
    - Collider werden deaktiviert/aktiviert
    - ein Gameobject mit allen Snapping previews wird gecyclet
    - Schritt in jede Funktion

    Steps:
    1. Mittelscheibe
    2. Au�enscheibe vorne
    3. Hauptscheibe
    4. Stifte einsetzen
    5. Federn einsetzen
    6. Schrauben festschrauben
    */

    public int assemblyStepCounter = 0;

    [Header("Snap Previews:")]
    public GameObject mittelscheibePreview;
    public GameObject aussenscheibeVornePreview;
    public GameObject hauptscheibePreview;
    public List<GameObject> federnPreview;
    public List<GameObject> stiftePreview;

    public Material previewGreen;
    public Material previewRed;

    [Header("Snap Trigger:")]
    public GameObject mittelscheibeTrigger;
    public GameObject aussenscheibeVorneTrigger;
    public GameObject hauptscheibeTrigger;
    public List<GameObject> federnTrigger;
    public List<GameObject> stifteTrigger;

    [Header("Real Gameobjects:")]
    public GameObject mittelscheibeObj;
    public GameObject aussenscheibeVorneObj;
    public GameObject aussenscheibeHintenObj;
    public GameObject hauptscheibeObj;
    public List<GameObject> federnObj;
    public List<GameObject> stifteObj;

    [Header("Arrows:")]
    public GameObject arrowMittelscheibe;
    public GameObject arrowAussenscheibeVorne;
    public GameObject arrowHauptscheibe;
    public GameObject arrowAkkuschrauber;
    public GameObject arrowFedern;
    public GameObject arrowStifte;


    private void Start()
    {
        NextStepDecider();
    }

    public void PreviewsRed()
    {
        mittelscheibePreview.GetComponent<Renderer>().material = previewRed;
        aussenscheibeVornePreview.GetComponent<Renderer>().material = previewRed;
        hauptscheibePreview.GetComponent<Renderer>().material = previewRed;
        foreach(GameObject obj in federnPreview)
        {
            obj.GetComponent<Renderer>().material = previewRed;
        }
        foreach(GameObject obj in stiftePreview)
        {
            obj.GetComponent<Renderer>().material = previewRed;
        }
    }

    public void PreviewsGreen()
    {
        mittelscheibePreview.GetComponent<Renderer>().material = previewGreen;
        aussenscheibeVornePreview.GetComponent<Renderer>().material = previewGreen;
        hauptscheibePreview.GetComponent<Renderer>().material = previewGreen;
        foreach (GameObject obj in federnPreview)
        {
            obj.GetComponent<Renderer>().material = previewGreen;
        }
        foreach (GameObject obj in stiftePreview)
        {
            obj.GetComponent<Renderer>().material = previewGreen;
        }
    }

    public void NextStepDecider()
    {
        switch (assemblyStepCounter)
        {
            case 0:
                InitialSetupMittelscheibe();
                break;
            case 1:
                AussenscheibeVorne();
                break;
            case 2:
                Hauptscheibe();
                break;
            case 3:
                Federn();
                break;
            case 4:
                Stifte();
                break;
            case 5:
                Akkuschrauber();
                break;
        }

        assemblyStepCounter++;
    }

    public void InitialSetupMittelscheibe()
    {
        // Alles was am Anfang gesetzt werden muss
    }

    public void AussenscheibeVorne()
    {
        mittelscheibePreview.SetActive(false);
        aussenscheibeVornePreview.SetActive(true);

        mittelscheibeTrigger.SetActive(false);
        aussenscheibeVorneTrigger.SetActive(true);

        arrowMittelscheibe.SetActive(false);
        arrowAussenscheibeVorne.SetActive(true);

        aussenscheibeVorneObj.tag = "CurrentActiveObject";
    }

    public void Hauptscheibe()
    {
        aussenscheibeVornePreview.SetActive(false);
        hauptscheibePreview.SetActive(true);

        aussenscheibeVorneTrigger.SetActive(false);
        hauptscheibeTrigger.SetActive(true);

        arrowAussenscheibeVorne.SetActive(false);
        arrowHauptscheibe.SetActive(true);

        hauptscheibeObj.tag = "CurrentActiveObject";
    }

    public void Federn()
    {
        hauptscheibePreview.SetActive(false);
        foreach(GameObject obj in federnPreview)
        {
            obj.SetActive(true);
        }

        hauptscheibeTrigger.SetActive(false);
        foreach (GameObject obj in federnTrigger)
        {
            obj.SetActive(true);
        }

        arrowHauptscheibe.SetActive(false);
        arrowFedern.SetActive(true);

        foreach(GameObject obj in federnObj)
        {
            obj.tag = "CurrentActiveObject";
        }
    }

    public void Stifte()
    {
        foreach (GameObject obj in federnPreview)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in stiftePreview)
        {
            obj.SetActive(true);
        }

        hauptscheibeTrigger.SetActive(false);
        foreach (GameObject obj in federnTrigger)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in stifteTrigger)
        {
            obj.SetActive(true);
        }

        arrowFedern.SetActive(true);
        arrowStifte.SetActive(false);

        foreach (GameObject obj in stifteObj)
        {
            obj.tag = "CurrentActiveObject";
        }
    }


    public void Akkuschrauber()
    {

    }
}
