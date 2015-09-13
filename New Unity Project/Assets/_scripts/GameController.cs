using UnityEngine;
using System.Collections.Generic;
using Utility;
using BladeCast;

public class GameController : MonoBehaviour {
	public GameObject playerObj;
	private List<GameObject> players = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		BCMessenger.Instance.RegisterListener ("connect", 0, this.gameObject, "HandleConnection");
		//BCMessenger.Instance.RegisterListener ("disconnect", 0, this.gameObject, "");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void HandleConnection(ControllerMessage msg) {
		// index of new hand
		int controllerIndex = msg.ControllerSource;     
		
	}
}
