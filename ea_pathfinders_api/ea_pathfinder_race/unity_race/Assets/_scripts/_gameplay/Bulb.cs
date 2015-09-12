
/*
Copyright (C) 2015 Electronic Arts Inc.  All rights reserved.
 
This software is solely licensed pursuant to the Hackathon License Agreement,
Available at:  http://www.eapathfinders.com/license
All other use is strictly prohibited.  
*/

using UnityEngine;
using System.Collections;

public class Bulb : MonoBehaviour {
	
	public Color m_onColor;
	public Color m_offColor;

	// Use this for initialization
	void Start () {
		IsOn = false;
	}
	
	/// <summary>
	/// Set to true to light it up. 
	/// </summary>
	public bool IsOn
	{
		set
		{
			Color newColor = (value) ? m_onColor : m_offColor;
			Renderer rend = GetComponent<Renderer>();
			Material mat = rend.material;
			mat.color = newColor;
			rend.material = mat;
		}
	}

}
