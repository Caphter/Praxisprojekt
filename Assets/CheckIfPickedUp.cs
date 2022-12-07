using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPickedUp : MonoBehaviour
{
    [Header("Object Moving:")]
    private Transform currentObjTrans;
    private Transform[] transformArr;
    private int arrayIndex = 0;
    public bool objectMoving = false;

    //[Header("Hand Distance:")]
    //public Vector3 vectorHand1;
    //public Vector3 vectorHand2;
    //public Vector3 vectorObject;
    //public bool handDistanceConstant = false;


    //public bool isPickedUp = false;

    private float intervallTimer = 0f;
    private float cycleTimer = 0f;
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        transformArr = new Transform[3];

        transformArr[arrayIndex] = this.gameObject.transform;
        
        
    }

    public void setNextArrayIndex()
    {
        transformArr[arrayIndex] = currentObjTrans;

        if(arrayIndex == 2)
        {
            arrayIndex = 0;
        }
        else
        {
            arrayIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // aktualisiert die Position abhängig der Position des Objects
        currentObjTrans = this.gameObject.transform;

        // Wenn die Position nicht mit der Ursprungsposition übereinstimmt, triggeren die Timer und Abfragen
        if (transformArr[arrayIndex] != this.gameObject.transform && !triggered)
        {
            triggered = true;
            arrayIndex++;
        }

        // Wenn eine Veränderung erkannt wurde, dann werden die Timer gestartet, und alle 1/6sec der entsprechend nächste Eintrag im Array gesetzt und alle 1/3 gecheckt, ob alle Transforms gleich sind, wenn nicht, ist das Objekt in Bewegung
        if (triggered)
        {
            intervallTimer += Time.deltaTime;
            cycleTimer += Time.deltaTime;

            if (intervallTimer >= 1 / 6f)
            {
                setNextArrayIndex();
                intervallTimer = 0;
            }

            if (cycleTimer >= 1 / 3f)
            {
                if (transformArr[0] == transformArr[1] && transformArr[0] == transformArr[2])
                {
                    objectMoving = false;
                    triggered = false;
                }
                else
                {
                    objectMoving = true;
                }
            }
        }

        
    }
}
