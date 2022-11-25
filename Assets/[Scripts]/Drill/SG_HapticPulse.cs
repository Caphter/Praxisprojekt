using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Keeps Track of a pulse pattern </summary>
public class SG_HapticPulse : MonoBehaviour
{
    /// <summary> The amount of times to pulse for. </summary>
    public int pulseFrequency = 10;

    /// <summary> the actual frequency with which to pulse. </summary>
    protected int pulseFreq = 10; //Hz

    /// <summary> Period; The time, in seconds, to perform one full finger sine wave pulse, in s. Based on frequency </summary>
    protected float f_T = 0;

    /// <summary> Half of the period, used to determine when the motors are on. </summary>
    protected float f_T_2;

    /// <summary> The sympTime corrected to the [0...f_T range] </summary>
    protected float f_cTime = 0;

    /// <summary> The time for which tp pulse, equal to (f_T / 2) only in ms. </summary>
    protected int pulseLength = 0;

    /// <summary> Indicates that the motors should be active at the moment (1/0). Toggles every f_T_2  </summary>
    protected int motorPulse = 0;

    /// <summary> The last haptic pulse as per the motor command. Used to only send the haptic feedback once. </summary>
    protected int lastHapticPulse = 0;

    /// <summary> Determiend if a command should be sent this frame. </summary>
    protected bool sendThisFrame = false;

    /// <summary> The pulse frequency. </summary>
    public int Frequency
    {
        get { return this.pulseFreq; }
        private set
        {
            this.pulseFreq = Mathf.Abs(value);
            this.CalcTimingVars();
        }
    }

    /// <summary> Check if the motors should be on. </summary>
    public bool MotorsOn { get { return this.MotorPulse == 1; } }

    /// <summary> Returns a 0 or 1 whether or not to pulse the motors. </summary>
    public int MotorPulse
    {
        get { return this.motorPulse; }
    }

    /// <summary> Time in ms, to pulse for based on our frequency. </summary>
    public int PulseTime
    {
        get { return this.pulseLength; }
    }

    public bool CanSend
    {
        get { return this.sendThisFrame; }
    }

    
    /// <summary> (re) calculate the timing variables when th </summary>
    private void CalcTimingVars()
    {
        if (this.pulseFreq != 0) //prevent NaN / Infinity errors
            f_T = 1 / (float)this.pulseFreq;
        else
            f_T = 0;

        f_T_2 = f_T * 0.5f;

        float f_T_4 = f_T * 0.25f; //one fourth of the time.
        pulseLength = (int)((f_T_4 * 2000) / 1.0f); //* 2 * 1000 = * 2000. Math.floor implied by the explicit cast to int

        this.f_cTime = 0;
    }


    private void UpdatePulse()
    {
        if (this.Frequency > 0) //prevent NaN / Infinity errors
        {
            this.f_cTime = Time.time % f_T;
            this.motorPulse = (f_cTime > 0 && f_cTime < f_T_2) ? 1 : 0; //indicates whether or not the finger pulse is on (1 / 0)
        }
        else
        {
            this.motorPulse = 0;
        }

        if (this.motorPulse == 1 && this.lastHapticPulse == 0)
            this.sendThisFrame = true;
        else
            this.sendThisFrame = false;

        this.lastHapticPulse = this.motorPulse;
    }

    //fires when the exposed value changes.
    private void OnValidate()
    {
        this.Frequency = this.pulseFrequency; //Update internal variable as well. 
    }

    // Use this for initialization
    void Start ()
    {
        this.CalcTimingVars();
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.UpdatePulse();
	}
}
