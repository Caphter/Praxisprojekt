using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RosSharp.RosBridgeClient

{
    public class DoorAngleSubscriber : UnitySubscriber<MessageTypes.BhsiInteraction.HapticInteraction>
    {
        public float doorAngle;
        public TurnDoorViaRossAngle turnDoorScript;
        private static float radiantDegreeConversionValue = 57.295779513f;

        //public GameObject Door;
        //bool msg_recieved;

        protected override void Start()
        {
            base.Start();
        }

        /*
        private void Update()
        {
            if (msg_recieved)
                process_message();
        }

        void process_message ()
        {
            Door.transform.eulerAngles = new Vector3(0, doorAngle, 0);
            msg_recieved = false;
        }
        */

        protected override void ReceiveMessage(MessageTypes.BhsiInteraction.HapticInteraction message)
        {
            doorAngle = (float)message.deflection.angular.z;
            turnDoorScript.currentAngle = -doorAngle * radiantDegreeConversionValue * (10f) + 1f;
            //msg_recieved = true;
        }
    }
}