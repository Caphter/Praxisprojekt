using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPickedUp : MonoBehaviour
{
    [Header("Object Moving:")]
    private Vector3 currentObjTrans;
    public Vector3[] transformArr;
    public int arrayIndex = 0;
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
        transformArr = new Vector3[3];

        transformArr[0].x = this.gameObject.transform.localPosition.x;
        transformArr[0].y = this.gameObject.transform.localPosition.y;
        transformArr[0].z = this.gameObject.transform.localPosition.z;
    }

    public void setNextArrayIndex()
    {
        transformArr[arrayIndex].x = currentObjTrans.x;
        transformArr[arrayIndex].y = currentObjTrans.y;
        transformArr[arrayIndex].z = currentObjTrans.z;


        if (arrayIndex >= 2)
        {
            arrayIndex = 0;
        }
        else if(arrayIndex < 2)
        {
            arrayIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // aktualisiert die Position abhängig der Position des Objects
        currentObjTrans = this.gameObject.transform.localPosition;

        // Wenn die Position nicht mit der Ursprungsposition übereinstimmt, triggeren die Timer und Abfragen
        if ((transformArr[0].x != currentObjTrans.x || transformArr[0].y != currentObjTrans.y || transformArr[0].z != currentObjTrans.z) && !triggered)
        {
            triggered = true;
        }

        // Wenn eine Veränderung erkannt wurde, dann werden die Timer gestartet, und alle 1/6sec der entsprechend nächste Eintrag im Array gesetzt und alle 1/3 gecheckt, ob alle Transforms gleich sind, wenn nicht, ist das Objekt in Bewegung
        if (triggered)
        {
            intervallTimer += Time.deltaTime;
            cycleTimer += Time.deltaTime;

            if (intervallTimer >= 1 / 3f)
            {
                setNextArrayIndex();
                intervallTimer = 0;
            }

            if (cycleTimer >= 1f)
            {
                if (transformArr[0].x == transformArr[1].x && transformArr[0].y == transformArr[2].y)
                {
                    objectMoving = false;
                    triggered = false;
                }
                else
                {
                    objectMoving = true;
                }

                cycleTimer = 0f;
            }
        }

        
    }
}
