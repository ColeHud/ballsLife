using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float speed;

	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(speed, speed * 2, speed) * Time.deltaTime);
	}
}
