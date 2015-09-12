
/*
Copyright (C) 2015 Electronic Arts Inc.  All rights reserved.
 
This software is solely licensed pursuant to the Hackathon License Agreement,
Available at:  http://www.eapathfinders.com/license
All other use is strictly prohibited.  
*/

using UnityEngine;
using System.Collections;
using System;
using BladeCast;


public class Racer : MonoBehaviour
{
    public float    k_thrust;               // The thrust given for  button hits
	public Color[] 	m_controlColors = new Color[4];

    // the racer can race... (not post race, etc)
    public bool     IsInPlay { get;  set; }

    private int m_controllerIndex = -1;


	void Start()
    {
		BCMessenger.Instance.RegisterListener("go_action", 0, this.gameObject, "HandleGoAction");
        IsInPlay = false;
    }

    /// <summary>
    /// Gets or sets the index of the controller.   Also sets color.
    /// </summary>
    public int ControllerIndex
    {
        get
        {
            return m_controllerIndex;
        }

        set
        {
            // set index and color...
            if (m_controllerIndex != value)
            {
                Renderer rend = GetComponent<Renderer>();
                Material mat = rend.material;

				mat.color = m_controlColors[value-1];
				rend.material = mat;
                m_controllerIndex = value;
            }
        }
    }

    /// <summary>
    /// Race director tells racer to get to starting position
    /// </summary>
    public void OnYourMark()
    { 
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    /// <summary>
    /// Race director tells this racer it has finished
    /// </summary>
	public void FinishedRace(int place)
    {
        IsInPlay = false;
		BCMessenger.Instance.SendToListeners("finished_race", "place", place,m_controllerIndex);
    }
   
    /// <summary>
    /// Handles the go action (message) -- A button hit from the controller
    /// </summary>
    private void HandleGoAction(ControllerMessage msg)
    {
        if (this.ControllerIndex == msg.ControllerSource)
        {
			ApplyForwardThrust(k_thrust);
        }
    }
	
    /// <summary>
    /// Push the object forward
    /// </summary>
    private void ApplyForwardThrust(float amount)
    {
        try
        {
            // apply force only in x/z
            Vector3 impulse = transform.rotation * Vector3.forward;
            AddForceAtPosition(impulse.normalized * amount, Vector3.zero);
        }
        catch(MissingReferenceException)
        {
            //watchdog at it, ignore
        }
    }
   
    /// <summary>
    /// Adds an impulse force at some point to the rigid body
    /// </summary>
    private void AddForceAtPosition(Vector3 impulse, Vector3 position)
    {
        try
        {
            this.GetComponent<Rigidbody>().AddForceAtPosition(impulse, position, ForceMode.Impulse);
        }
        catch(MissingReferenceException)
        {
            //Watchdog ate it, ignore
        }
    }

}
