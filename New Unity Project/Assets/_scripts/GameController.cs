using UnityEngine;
using System.Collections.Generic;
using Utility;
using BladeCast;

public class GameController : MonoBehaviour {
	public GameObject[] playerDB;
	public Dictionary<int, GameObject> players;

	// Use this for initialization
	void Start () {
		BCMessenger.Instance.RegisterListener ("connect", 0, this.gameObject, "HandleConnect");
		BCMessenger.Instance.RegisterListener ("disconnect", 0, this.gameObject, "HandleDisconnect");
	}

	private void HandleConnection(ControllerMessage msg) {
		int controllerIndex = msg.ControllerSource; 
		players.Add(controllerIndex, (GameObject)Instantiate(playerDB[controllerIndex], new Vector3(0f, 5f, 0f), Quaternion.identity)); 
	}

	private void HandleDisconnect(ControllerMessage msg) {
		int controllerIndex = msg.ControllerSource;
		Destroy (players [controllerIndex]);
		players.Remove (controllerIndex);
	}
}
