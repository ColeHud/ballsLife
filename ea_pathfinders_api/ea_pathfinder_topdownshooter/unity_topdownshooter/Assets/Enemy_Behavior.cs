using UnityEngine;
using System.Collections;

public class Enemy_Behavior : MonoBehaviour {

	public float movementSpeed;
	public float lifeTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMoveTowardPlayer ();
		UpdateLifetime ();
	}

	void UpdateMoveTowardPlayer() {
	
		GetComponent<Rigidbody> ().AddForce (this.transform.forward * movementSpeed);
	}

	void UpdateLifetime() {
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0) {
			Destroy(this.gameObject);
		}
	}
}
