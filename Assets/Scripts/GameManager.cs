using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int level = 0;

	public int checkpointsPassed = 0;

	private int playerScore = 0;

	public int PlayerScore{
		get{ return playerScore; }
		set{ playerScore = value; }
	}

	private GameObject player;

	private List<Transform> spawnPoints = new List<Transform>();

	// Use this for initialization even if game has not been set up yet, or this script is not enabled.
	void Awake() {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		//Get Player
		player = GameObject.Find("Player");

		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("EnemySpawn");

		for (int i=0; i<spawners.Length; ++i) {
			spawnPoints.Add(spawners[i].transform);
		}



		InitGame ();

	}

	void InitGame()
	{
		//GunManager, set gunlevel
	}

	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GameOver()
	{
		enabled = false;
	}

	public void UpdateScore(int score)
	{
		playerScore += score;
	}
}
