using UnityEngine;
using System;
using System.Collections;
using BladeCast;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GameObject bigBoom;
	public GameObject spark;
	public GameObject boostCloud;
	public float boostSpeed;
	public float boostDur; //Boost Durraction
	public AudioClip death;
	public AudioClip bump;
	public AudioClip boostSound;
	public int lives;

	private Rigidbody rb;
	private AudioSource source;
	private int boostCounter;
	private bool boosting;
	private Vector3 originSize;

	void Start () {
		BCMessenger.Instance.RegisterListener ("boost", 0, this.gameObject, "StartBoost");
		BCMessenger.Instance.RegisterListener ("gyro" ,0, this.gameObject, "MovePlayer");
		//BCMessenger.Instance.RegisterListener ("
		rb = GetComponent<Rigidbody>();	
		boostCounter = 0;
		boosting = false;
		originSize = this.transform.localScale;
		source = GetComponent<AudioSource> ();
	}

	void FixedUpdate () {
		boostCounter++;
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		bool boost = Input.GetKeyUp ("space");

		if (boosting && boostCounter > boostDur) {
			StopBoost();
		}

		if (!boosting && boost) {
			StartBoost ();
		}

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.AddForce(movement * speed);
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Death")) {
			rb.velocity = rb.velocity * 0.0f;
			GameObject temp = (GameObject)Instantiate(bigBoom, this.transform.position, Quaternion.identity); 
			source.PlayOneShot(death, 0.5f);
			lives--;
			if(lives >= 0){
				rb.position = new Vector3(0.0f, 8f, 0.0f);
			}
		}
	}

	void OnCollisionEnter (Collision other) {
		Debug.Log ("ColEnter");
		if (other.gameObject.CompareTag ("Player")) {
			GameObject temp = (GameObject)Instantiate(spark, this.transform.position, Quaternion.identity); 
			source.PlayOneShot(bump, 0.1f);
		}
	}

	void StartBoost () {
		rb.AddExplosionForce (boostSpeed,rb.position - rb.velocity, 0.0f, 1f, ForceMode.Impulse);
		GameObject temp = (GameObject)Instantiate (boostCloud, this.transform.position, Quaternion.identity);
		//this.transform.localScale = 2f * this.transform.localScale;
		source.PlayOneShot (boostSound, 0.5f);
		boosting = true;
		boostCounter = 0;
	}

	void StopBoost () {
		rb.velocity = rb.velocity * 0.5f;
		//this.transform.localScale = this.transform.localScale / 2f;
		boosting = false;
	}

	void MovePlayer(ControllerMessage msg){
		float x = 0;
		float z = 0;
		if (msg.Payload.HasField ("beta")) {
			z = System.Convert.ToSingle(msg.Payload.GetField("beta").ToString());
		} else {
			print ("Vertical tilt not detected");
		}
		if (msg.Payload.HasField ("gamma")) {
			x = System.Convert.ToSingle(msg.Payload.GetField("gamma").ToString());
		} else {
			print ("Horizontal tilt not detected");
		}
		Vector3 move = new Vector3 (x, 0.0f, z);
		rb.AddForce (move * speed);
	}
}
