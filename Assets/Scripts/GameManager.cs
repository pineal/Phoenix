using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int level = 0;
	public int stage = 0;

	public List<GameObject> Enemies;

	public int checkpointsPassed = 0;

	private int playerScore = 0;

	public int PlayerScore{
		get{ return playerScore; }
		set{ playerScore = value; }
	}

	private int spawnedEnemies = 0, destroyedEnemies = 0, needToSpawn = 0;
	private bool spawnBigBoss = false;

	private GameObject player;

	private int gameStatus = 0;		//0 - inactive, 1 - active, 2 - end of game

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
			//Making sure that the spawn point is at the proper Depth 
			Vector3 pos = spawners[i].transform.position;
			pos.z = 4f;
			spawners[i].transform.position = pos;

			spawnPoints.Add(spawners[i].transform);
		}

		InitGame ();

	}

	void InitGame()
	{


		// GunManager, set gunlevel


		//Activate Game
		gameStatus = 1;

		UpdateStage ();
	}

	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.
	void Start () {


	
	}

	//Layout of Level
	void UpdateStage ()		
	{
		switch (level) {
		case 0:		// Level 0
		{
			switch (stage) {
			case 0:		// Stage 0
			{
				if (needToSpawn == 0) {
					++needToSpawn;
					break;
				} 
				else if(needToSpawn == 1 || needToSpawn == 3) {
					needToSpawn += 2;
					break;
				}
				else
				{
					ResetStageVars();
					++stage;
				}
			}
			goto case 1;

			case 1:		// Stage 1
			{
				if (needToSpawn == 0 || needToSpawn == 3) {
					needToSpawn += 3;
					break;
				}
				else
				{
					ResetStageVars();
					++stage;
				}
			}
			goto case 2;
			
			case 2:		// Stage 2
			{
				if (needToSpawn == 0 || needToSpawn == 7) {
					needToSpawn += 3;
					break;
				}
				else if(needToSpawn == 3) {
					needToSpawn += 4;
					break;
				}
				else
				{
					ResetStageVars();
					++stage;
				}
			}
			goto case 3;

			case 3:		// Stage 3
			{
				if (needToSpawn == 0) {
					needToSpawn += 2;
					spawnBigBoss = true;
					break;
				}
				else
				{
					ResetStageVars();
					++stage;
				}
			}
				break;
			}

		}
		
		if(stage < 4)
			break;

		goto default;

		default:
			//End of Game
			GameOver();
			break;
		}

		SpawnEnemies ();
	}

	void ResetStageVars()
	{
		needToSpawn = spawnedEnemies = destroyedEnemies = 0;
	}

	public void EnemyDestroyed()
	{
		++destroyedEnemies;

		if ( (spawnedEnemies == destroyedEnemies) && (destroyedEnemies == needToSpawn) ) {
			UpdateStage ();
		}

	}

	void SpawnEnemies()
	{
		if (needToSpawn == spawnedEnemies)
			return;

		//GameObject obj = (GameObject) Instantiate(Enemies[0], spawnPoints[0].position, Quaternion.identity);

		StopAllCoroutines ();

		StartCoroutine (EnemySpawner());

	}

	IEnumerator EnemySpawner ()
	{
		for (int i=spawnedEnemies; i<needToSpawn; ++i) {
			//Get Spawn Point
			int spawnIndex = Random.Range(0, spawnPoints.Count);

			//Instantiate
			GameObject obj = (GameObject) Instantiate(Enemies[0], spawnPoints[spawnIndex].position, Quaternion.identity);
			obj.SetActive(true);

			++spawnedEnemies;

			yield return new WaitForSeconds(2f);
		}
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
