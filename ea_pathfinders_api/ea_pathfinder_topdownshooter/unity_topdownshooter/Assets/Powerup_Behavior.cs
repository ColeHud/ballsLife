using UnityEngine;
using System.Collections;

public class Powerup_Behavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Bullet") {
			GameObject.FindGameObjectWithTag("Player").SendMessage("EnablePowerup");
			Destroy(this.gameObject);
		}
	}
}
