
/*
Copyright (C) 2015 Electronic Arts Inc.  All rights reserved.
 
This software is solely licensed pursuant to the Hackathon License Agreement,
Available at:  http://www.eapathfinders.com/license
All other use is strictly prohibited.  
*/


using UnityEngine;
using System.Collections;
using Utility;
using BladeCast;

public class RaceTower : MonoBehaviour
{
	public Bulb[]				m_bulbs = new Bulb[5];
	private SimpleStateMachine  m_stateMachine;

	// Use this for initialization
	void Start ()
	{
		BCMessenger.Instance.RegisterListener("start_new_game", 0, this.gameObject, "HandleStartNewGame");      
		BCMessenger.Instance.RegisterListener("start_pre_race", 0, this.gameObject, "HandleStartPreRace");      
		BCMessenger.Instance.RegisterListener("start_countdown", 0, this.gameObject, "HandleStartCountdown");      

		m_stateMachine = new SimpleStateMachine ("RaceTower");
		m_stateMachine.SetState(UpdatePreStart);	
	}

	// Update is called once per frame
	void Update () 
	{
		m_stateMachine.Update (Time.deltaTime);
	}

	//-----------------------------------------------------------------
	// States...
	//-----------------------------------------------------------------

	private void UpdatePreStart (float stateTime)
	{
		if (stateTime == 0.0)
		{
			foreach(Bulb bulb in m_bulbs)
				bulb.IsOn = false;
		}
	}

	private void UpdatePreRace (float stateTime)
	{
		if (stateTime == 0.0)
			m_bulbs[0].IsOn = true;
	}

	private void UpdateCountDown3 (float stateTime)
	{
		if (stateTime == 0.0)
			m_bulbs[1].IsOn = true;
		else
			if(stateTime >= 1.0f)
				m_stateMachine.SetState (UpdateCountDown2);
	}

	private void UpdateCountDown2 (float stateTime)
	{
		if (stateTime == 0.0)
			m_bulbs[2].IsOn = true;
		else
			if(stateTime >= 1.0f)
				m_stateMachine.SetState (UpdateCountDown1);
	}

	private void UpdateCountDown1 (float stateTime)
	{
		if (stateTime == 0.0)
			m_bulbs[3].IsOn = true;
		else
			if(stateTime >= 1.0f)
				m_stateMachine.SetState (UpdateGo);
	}

	private void UpdateGo (float stateTime)
	{
		if (stateTime == 0.0) 
		{
			m_bulbs [4].IsOn = true;
			BCMessenger.Instance.SendToListeners ("start_race", -1);
		}
	}

	//... message receivers...
	private void HandleStartNewGame(ControllerMessage msg)
	{
		m_stateMachine.SetState(UpdatePreStart);
	}

	private void HandleStartPreRace(ControllerMessage msg)
	{
		m_stateMachine.SetState(UpdatePreRace);
	}

	private void HandleStartCountdown(ControllerMessage msg)
	{
		m_stateMachine.SetState(UpdateCountDown3);
	}
	
}
