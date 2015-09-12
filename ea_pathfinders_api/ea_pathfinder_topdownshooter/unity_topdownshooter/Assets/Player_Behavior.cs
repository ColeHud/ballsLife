using UnityEngine;
using System.Collections;
using BladeCast;

public class Player_Behavior : MonoBehaviour {

	public GameObject bullet_Prefab;
	public float spawnOffset;
	
	public float currentBulletShootTime;
	public float maxBulletShootTime;

	bool powerEnabled;
	public float currentPowerEnabledTime;
	public float maxPowerEnabledTime;
	bool shootState;

	// Use this for initialization
	void Start () {
		InitControllerListeners ();
	}
	
	// Update is called once per frame
	void Update () {
		//UpdateRotation ();
		UpdateShoot ();
		HandlePowerup ();
	}

	void InitControllerListeners() {
		BCMessenger.Instance.RegisterListener("connect", 0, this.gameObject, "HandleControllerRegister");
		BCMessenger.Instance.RegisterListener("angle", 0, this.gameObject, "HandleRotate_ControllerInput");
		BCMessenger.Instance.RegisterListener("powerup", 0, this.gameObject, "HandlePowerup_ControllerInput");
		BCMessenger.Instance.RegisterListener("shoot", 0, this.gameObject, "HandleShoot_ControllerInput");
	}

	void HandleControllerRegister() {
		print ("Connected to controller");
	}

	void HandleRotate_ControllerInput(ControllerMessage msg) {
		if (msg.Payload.HasField ("angle")) {
			string angle_value_raw = msg.Payload.GetField("angle").ToString();
			float angle_value_parsed;
			if(float.TryParse(angle_value_raw, out angle_value_parsed)) {
				SetRotation(angle_value_parsed);
			}
		} else {
			print ("angle field did not exist");
		}
	}

	void HandlePowerup_ControllerInput(ControllerMessage msg) {
		powerEnabled = true;
		currentPowerEnabledTime = maxPowerEnabledTime;

	}

	void HandleShoot_ControllerInput(ControllerMessage msg) {
		if (msg.Payload.HasField ("state")) {
			string shoot_state_raw = msg.Payload.GetField("state").ToString();
			int shoot_state_parsed;
		
			if(int.TryParse(shoot_state_raw, out shoot_state_parsed)) {
				shootState = (shoot_state_parsed > 0);
			}
		} else {
			print ("shoot field did not exist");
		}
	}

	void EnablePowerup() {
		BCMessenger.Instance.SendToListeners("enable_powerup_button", -1);
	}

	void HandlePowerup() {
		if (currentPowerEnabledTime > 0) {
			currentPowerEnabledTime -= Time.deltaTime;
			if(currentPowerEnabledTime <= 0) {
				powerEnabled = false;
				BCMessenger.Instance.SendToListeners("disable_powerup_button", -1);
			}
			BCMessenger.Instance.SendToListeners ("set_powerup_button_time", "time", Mathf.CeilToInt (currentPowerEnabledTime), -1);
			print ("Powerup enabled time: " + currentPowerEnabledTime);
		}

		if (powerEnabled) {
			print ("Powered Up");
		}
	}

	void SetRotation(float angle) {
		Quaternion rot = new Quaternion ();
		rot.eulerAngles = new Vector3(0, -Mathf.Rad2Deg * angle,0);
		this.transform.rotation = rot;
	}

	void UpdateRotation() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		this.transform.LookAt (new Vector3(mousePos.x, this.transform.position.y, mousePos.z));
	}

	void UpdateShoot() {
		if (shootState && currentBulletShootTime <= 0) {
			currentBulletShootTime = maxBulletShootTime;
			Instantiate (bullet_Prefab, this.transform.position + this.transform.forward * spawnOffset, this.transform.rotation);
		} else {
			currentBulletShootTime -= Time.deltaTime;
		}
	}
}
