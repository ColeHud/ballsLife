
/*
Copyright (C) 2015 Electronic Arts Inc.  All rights reserved.
 
This software is solely licensed pursuant to the Hackathon License Agreement,
Available at:  http://www.eapathfinders.com/license
All other use is strictly prohibited.  
*/

using UnityEngine;
using System.Collections.Generic;
using Utility;
using BladeCast;

public class RaceMaster : MonoBehaviour 
{
    public  Racer               m_racerPrototype;
    private List<Racer>         m_racers = new List<Racer> ();
    private List<Racer>         m_finishers;

    private SimpleStateMachine  m_stateMachine;

    // constants...
    private Vector3             k_firstRacerPos = new Vector3 (-20.0f, 0.0f, -55.0f);
    private Vector3             k_racerOffset = new Vector3 (15.0f, 0.0f, 0.0f);
    private float               k_finishingTime = 3.0f;
    private float               k_postRaceTime = 5.0f;

	void Start()
    {  
		BCMessenger.Instance.RegisterListener("connect", 0, this.gameObject, "HandleConnection");      
		BCMessenger.Instance.RegisterListener("start_race", 0, this.gameObject, "HandleStartRace");  

        ChildCollider.AttachChildEnterDelegate(this.transform, OnRaceTrigger);

        m_stateMachine = new SimpleStateMachine ("RaceMaster");
        m_stateMachine.SetState(UpdateWaitForPlayers);
    }
    
    // Update is called once per frame
    void Update () 
    {
        m_stateMachine.Update (Time.deltaTime);
    }

	//-----------------------------------------------------------------
	// States...
	//-----------------------------------------------------------------

	/// <summary>
	/// Updates the start new game.
	/// Put game-wide inits here...
	/// </summary>
	private void UpdateStartNewGame (float stateTime)
	{
		BCMessenger.Instance.SendToListeners("start_new_game", -1);
		m_stateMachine.SetState (UpdateWaitForPlayers);
	}
	
	private void UpdateWaitForPlayers (float stateTime)
	{
		if (stateTime == 0.0f)
		{
			// put racers on starting line and tell controllers...
			foreach (Racer racer in m_racers)
			{	
				SetRacerPos(racer);
				racer.OnYourMark();
			}
		}

		if (m_racers.Count != 0) 
		{
			BCMessenger.Instance.SendToListeners ("start_pre_race", -1);
			m_stateMachine.SetState (WaitForAdditionalPlayers);
		}
	}
	
	private void WaitForAdditionalPlayers(float stateTime)
	{
		if (stateTime >= Settings.Game.m_newPlayerWaitTime)
		{
			// race tower does final countdown..
			BCMessenger.Instance.SendToListeners("start_countdown", -1);
			m_stateMachine.SetState (UpdatePreRace);
		}
	}

	/// <summary>
	/// Waiting for tower to count down...
	/// </summary>
	private void UpdatePreRace(float stateTime)
	{
	}

    /// <summary>
    /// During the race
    /// </summary>
    private void UpdateRacing(float stateTime)
    {
        if (stateTime == 0.0f)
        {
			m_finishers = new List<Racer> ();
            foreach (Racer racer in m_racers)
                racer.IsInPlay = true;
        }

		if(m_finishers.Count > 0)
			m_stateMachine.SetState (UpdateFinishingRace);
    }
    
    /// <summary>
    /// Finishing the race... (at least 1 racer has crossed... timeout the rest)
    /// </summary>
    private void UpdateFinishingRace(float stateTime)
    { 
        // after timeout... handle any non-finishers.
        if (stateTime >= k_finishingTime)
        {  
            foreach (Racer racer in m_racers)
				FinishRacer(racer);
        }

        // all racers have crossed finish line...
        if(m_finishers.Count == m_racers.Count)
			m_stateMachine.SetState(UpdatePostRace);
    }
	
    /// <summary>
    /// Wait for start of new game
    /// </summary>
    public void UpdatePostRace(float stateTime)
    {
        if (stateTime > k_postRaceTime)
			m_stateMachine.SetState(UpdateStartNewGame);
    }


	// message handlers...
    private void HandleConnection(ControllerMessage msg)
    {
        // index of new hand
        int controllerIndex = msg.ControllerSource;     
        
        Racer racer = m_racers.Find (x => x.ControllerIndex == controllerIndex);
        if (racer == null) 
        {
            racer = CreateRacer (controllerIndex);
            SetRacerPos(racer);
        } 
    }

	private void HandleStartRace(ControllerMessage msg)
	{
		m_stateMachine.SetState(UpdateRacing);
	}


    /// <summary>
    /// Called from starting line or finishing line trigger when racer crosses...
    /// </summary>
    private void OnRaceTrigger(Collider collider, Transform childTrans)
    {
        Racer racer = collider.gameObject.GetComponent<Racer>();
        if (childTrans.name == "starting_line")
        {
			// racer crosses the starting line..
        } 
        else
        if (childTrans.name == "finishing_line")        
            FinishRacer(racer);
    }
	
    private Racer CreateRacer(int controllerIndex)
    {
        Racer racer = (Racer)Instantiate(m_racerPrototype, Vector3.zero, Quaternion.identity);
        racer.transform.SetParent (this.transform);
        racer.gameObject.name = "racer_player_" + controllerIndex.ToString ();
        racer.ControllerIndex = controllerIndex;
        
        m_racers.Add(racer);
        return racer;
    }

    private void SetRacerPos(Racer racer)
    {
        Vector3 pos = k_firstRacerPos + k_racerOffset * (float)(racer.ControllerIndex - 1);
        racer.transform.localPosition = pos;
    }

    /// <summary>
    /// Racer has crossed the finish line (or has otherwise finished)
    /// </summary>
    /// <param name="racer">Racer.</param>
    private void FinishRacer(Racer racer)
    {
		if (!m_finishers.Contains (racer)) 
		{
			m_finishers.Add (racer);
			racer.FinishedRace (m_finishers.Count);
		}
    }
}
