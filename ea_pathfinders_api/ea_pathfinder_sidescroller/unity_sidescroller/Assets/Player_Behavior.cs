using UnityEngine;
using System.Collections;
using BladeCast;

public enum MOVE_DIRECTION { 
	LEFT, 
	RIGHT 
};

public enum BUTTON_STATE {
	UP,
	DOWN
};

public class Player_Behavior : MonoBehaviour {

	public float speed;
	BUTTON_STATE left_button;
	BUTTON_STATE right_button;


	// Use this for initialization
	void Start () {
		BCMessenger.Instance.RegisterListener("connect", 0, this.gameObject, "HandleControllerRegister");
		BCMessenger.Instance.RegisterListener("move_direction", 0, this.gameObject, "HandleMoveDirection");

		left_button = right_button = BUTTON_STATE.UP;
	}

	void HandleControllerRegister(ControllerMessage msg) {
		print ("Controller has joined the game");

		int controllerIndex = msg.ControllerSource;

		if (controllerIndex > 1) {
			print ("Too many controllers - bugger off");
		}

	}

	void HandleMoveDirection(ControllerMessage msg) {
	
		if (msg.Payload.HasField ("move_direction")) {
			print ("[HandleMoveDirection] Inside that cool if statement");
			string dir_value_raw = msg.Payload.GetField("move_direction").ToString();
			string button_state_raw = msg.Payload.GetField("button_state").ToString();
			int dir_value_parsed;
			int button_state_parsed;
			if (int.TryParse(dir_value_raw, out dir_value_parsed) && int.TryParse(button_state_raw, out button_state_parsed)) {
				HandleInput ((MOVE_DIRECTION)dir_value_parsed, (BUTTON_STATE)button_state_parsed);
			} else {
				print ("Well... It's not parseable");
			}

		} else {
			print ("move_direction field did not exist");
		}
	}

	// Update is called once per frame
	void Update () {
		ApplyInput ();
	}

	void HandleInput(MOVE_DIRECTION user_input, BUTTON_STATE button_state) {

		//print ("[HandleInput] Entered with user input: " + user_input);

		if (user_input == MOVE_DIRECTION.LEFT) {
			left_button = button_state;
		} else if (user_input == MOVE_DIRECTION.RIGHT) {
			right_button = button_state;
		} else {
			print ("Received incorrect values for input");

		}
	}

	void ApplyInput() {
		
		//print ("[ApplyInput] Entered");
		
		if (left_button == BUTTON_STATE.DOWN) {
			GetComponent<Rigidbody> ().AddForce (new Vector3 (-speed, 0, 0));
		} else if (right_button == BUTTON_STATE.DOWN) {
			GetComponent<Rigidbody> ().AddForce (new Vector3 (speed, 0, 0));
		} else {
			print ("Received incorrect values for button_state");
			
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Spike") {
			Die();
		}
		if (collision.gameObject.tag == "LevelGate") {
			Win(collision.gameObject.GetComponent<LevelGate_Behavior>().levelToLoad);
		}
	}

	void Win(string level) {
		Application.LoadLevel (level);
	}

	void Die() {
		Application.LoadLevel ("Level1");
	}
}
