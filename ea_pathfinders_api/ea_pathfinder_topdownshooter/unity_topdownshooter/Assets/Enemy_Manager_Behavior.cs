using UnityEngine;
using System.Collections;

public class Enemy_Manager_Behavior : MonoBehaviour {

	public GameObject enemy_Prefab;

	public float currentEnemySpawnTime;
	public float maxEnemySpawnTime;

	public float minEnemySpawnDistance;
	public float maxEnemySpawnDistance;

	Vector3 playerLocation;

	// Use this for initialization
	void Start () {
		playerLocation = GameObject.FindGameObjectWithTag ("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateSpawnEnemy ();
	}

	void UpdateSpawnEnemy() {

		if (currentEnemySpawnTime <= 0) {
			currentEnemySpawnTime = maxEnemySpawnTime;

			Vector3 randomLocation = playerLocation + new Vector3(Random.Range(minEnemySpawnDistance, maxEnemySpawnDistance), 0, Random.Range(minEnemySpawnDistance, maxEnemySpawnDistance));

			GameObject enemy = (GameObject)Instantiate (enemy_Prefab, randomLocation, new Quaternion());
			enemy.transform.LookAt(playerLocation);
		} else {
			currentEnemySpawnTime -= Time.deltaTime;
		}

	}
}
