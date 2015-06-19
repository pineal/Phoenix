using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int level = 0;
	public int stage = 0;

	public GameObject canvas;
	public Text popupText;
	public GameObject pauseUI;

	private float topOfActiveScreen = 0.0f;
	private float rightOfActiveScreen = 0.0f;

	public float TopOfActiveScreen{
		get{ return topOfActiveScreen; }
	}
	public float RightOfActiveScreen{
		get{ return rightOfActiveScreen; }
	}

	public List<GameObject> Enemies;
	public List<GameObject> BigBoss;

	public int checkpointsPassed = 0;

	private int playerScore = 0;
	public int PlayerScore{
		get{ return playerScore; }
		set{ playerScore = value; }
	}

	private Vector3 playerSpawnPos = new Vector3 (0f, 0f, 0f);

	private int spawnedEnemies = 0, destroyedEnemies = 0, needToSpawn = 0;
	private bool spawnBigBoss = false;

	private GameObject player = null;
	public GameObject Player{
		get{ return player; }
	}
	[HideInInspector]public GunManager playerGunMgr = null;
	private int playerInitHealth = 0;
	public int PlayerInitHealth{
		get{ return playerInitHealth; }
	}

	private List<Transform> spawnPoints = new List<Transform>();

	private bool isPopupPublishing = false;


	public enum Mode {NOT_IN_STAGE, IN_STAGE, LEVEL_WIN, LEVEL_LOSS, RESPAWNING, LEVEL_START};

	private Mode playMode;

	public int PlayMode {
		get{ return (int)playMode;}
	}

	// Use this for initialization even if game has not been set up yet, or this script is not enabled.
	void Awake() {

		if (instance == null)
			instance = this;
		else if (instance != this) {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		//Get Player
		player = GameObject.Find("Player");
		playerGunMgr = player.GetComponentInChildren<GunManager> ();
		playerSpawnPos = player.transform.position;
		playerInitHealth = player.GetComponent<DamageScript> ().health;


		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("EnemySpawn");

		for (int i=0; i<spawners.Length; ++i) {
			//Making sure that the spawn point is at the proper Depth 
			Vector3 pos = spawners[i].transform.position;
			pos.z = 4f;
			spawners[i].transform.position = pos;

			spawnPoints.Add(spawners[i].transform);
		}

		topOfActiveScreen = Camera.main.orthographicSize - 1.5f;
		rightOfActiveScreen = Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height);

		InitGame ();

	}

	void InitGame()
	{
		StopAllCoroutines();
		playMode = Mode.NOT_IN_STAGE;
		stage = 0;

		spawnedEnemies = 0;
		destroyedEnemies = 0;
		needToSpawn = 0;
		spawnBigBoss = false;

		// GunManager, set gunlevel
		playerGunMgr.gunLevel = 128;
		playerGunMgr.ActivatePlayerGun();


		//Reset Popup Text UI
		isPopupPublishing = false;
		popupText.gameObject.SetActive(false);

		//Set Play Mode
		playMode = Mode.LEVEL_START;


	}

	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.
	void Start () {



	}

	void Update()
	{
		if (Application.loadedLevel == 0) {
			StopAllCoroutines ();	
		} else if (playMode == Mode.LEVEL_START){
			playMode = Mode.NOT_IN_STAGE;
			StartCoroutine("UpdateStage");
		}

	}

	//Layout of Level
	IEnumerator UpdateStage ()		
	{
		switch (level) {
		case 0:		// Level 0
		{
			switch (stage) {
			case 0:		// Stage 0
			{
				while(playMode == Mode.NOT_IN_STAGE)
				{
					while (isPopupPublishing)
						yield return null;

					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Welcome to Pandora, Soldier!");


					while (isPopupPublishing)
						yield return null;

					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "You've got incoming hostiles!");


					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Get off your Auto pilot!");

					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Here we go ...");


					while (isPopupPublishing)
						yield return null;
					playMode = Mode.IN_STAGE;
				}

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
					playMode = Mode.NOT_IN_STAGE;
				}
			}
			goto case 1;

			case 1:		// Stage 1
			{
				//Pre-Stage Messages
				while(playMode == Mode.NOT_IN_STAGE)
				{
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Good Job!");


					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Take a breather!");

					yield return new WaitForSeconds(3f);

					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Second Wave Incoming!");

					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "All Hands on Deck!");
					
					
					while (isPopupPublishing)
						yield return null;
					playMode = Mode.IN_STAGE;
				}

				if ( (playerGunMgr.gunLevel & 1) == 0 )		//LSB == Gun[2];
				{
					playerGunMgr.gunLevel = playerGunMgr.gunLevel | 129;
					playerGunMgr.ActivatePlayerGun();
				}

				if (needToSpawn == 0 || needToSpawn == 3) {
					needToSpawn += 3;
					break;
				}
				else
				{
					ResetStageVars();
					++stage;
					playMode = Mode.NOT_IN_STAGE;
				}
			}
			goto case 2;
			
			case 2:		// Stage 2
			{
				//Pre-Stage Messages
				while(playMode == Mode.NOT_IN_STAGE)
				{
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "You've got some moves, hunter!");
					
					
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "We could use someone like you");
					
					yield return new WaitForSeconds(3f);
					
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Next Wave Incoming!");
					
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Hands on Deck!");
					
					
					while (isPopupPublishing)
						yield return null;
					playMode = Mode.IN_STAGE;
				}

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
					playMode = Mode.NOT_IN_STAGE;
				}
			}
			goto case 3;

			case 3:		// Stage 3
			{
				//Pre-Stage Messages
				while(playMode == Mode.NOT_IN_STAGE)
				{
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Whoa ...");
					
					
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "You've got a big one heading your way");
					
					yield return new WaitForSeconds(3f);
					
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Incoming!");
					
					while (isPopupPublishing)
						yield return null;
					
					isPopupPublishing = true;
					StartCoroutine("PublishPopupReal", "Hands on Deck!");
					
					
					while (isPopupPublishing)
						yield return null;
					playMode = Mode.IN_STAGE;
				}

				if (needToSpawn == 0 || needToSpawn == 2) {
					needToSpawn = 2;
					if (!spawnBigBoss)
						spawnBigBoss = true;

					break;
				}
				else
				{
					ResetStageVars();
					//++stage;
					//playMode = Mode.NOT_IN_STAGE;
				}
			}
				break;
			}

		}
		
		//!!! Probably do not need this anymore
		if(stage < 4)
			break;
		else
		{
			playMode = Mode.LEVEL_WIN;
		}


		goto default;

		default:
			//End of Game

			GameOver();
			break;
		}

		SpawnEnemies ();

		yield return null;
	}

	void ResetStageVars()
	{
		needToSpawn = spawnedEnemies = destroyedEnemies = 0;
	}

	public void EnemyDestroyed(string tag, Vector3 pos)
	{
		if (tag == "BigBoss") {
			++stage;

			playMode = Mode.LEVEL_WIN;

			StopCoroutine("UpdateStage");
			StartCoroutine("UpdateStage");

			return;
		}

		PickupManager.instance.spawnPickup (pos);

		++destroyedEnemies;

		if ( (spawnedEnemies == destroyedEnemies) && (destroyedEnemies == needToSpawn) ) {
			StopCoroutine("UpdateStage");
			StartCoroutine("UpdateStage");
		}

	}

	void SpawnEnemies()
	{
		if (needToSpawn == spawnedEnemies)
			return;

		//GameObject obj = (GameObject) Instantiate(Enemies[0], spawnPoints[0].position, Quaternion.identity);

		StopCoroutine ("EnemySpawner");

		StartCoroutine ("EnemySpawner");

	}

	IEnumerator EnemySpawner ()
	{
//		if (!popupText.gameObject.activeSelf)
//			yield return null;

		yield return new WaitForSeconds (3f);

		if (spawnBigBoss) {
			GameObject obj = (GameObject)Instantiate (BigBoss[0], spawnPoints[0].position, Quaternion.identity);
			BossMove script = obj.GetComponent<BossMove>();

			if (script != null)
			{
				script.MoveType = 0;
			}

			obj.SetActive(true);

			spawnBigBoss = false;
		}

		for (int i=spawnedEnemies; i<needToSpawn; ++i) {
			//Get Spawn Point
			int spawnIndex = Random.Range(0, spawnPoints.Count);

			//Instantiate
			GameObject obj = (GameObject) Instantiate(Enemies[0], spawnPoints[spawnIndex].position, Quaternion.identity);
			EnemyAir script = obj.GetComponent<EnemyAir>();

			if(script != null)
			{
				script.MoveType = Random.Range(0,2);
			}

			obj.SetActive(true);

			++spawnedEnemies;

			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator PublishPopupReal(string text)
	{
		RectTransform rt = popupText.gameObject.GetComponent<RectTransform>();

		popupText.text = text; 

		while (true) {

			popupText.gameObject.SetActive (true);
			float scale = 0.01f;
			Vector3 scaleVec = new Vector3 (scale, scale, 1f);
			rt.localScale = scaleVec;

			while (rt.localScale.x < 1) {
				scale += Time.deltaTime * 3;
				scaleVec.x = scale;
				scaleVec.y = scale;

				rt.localScale = scaleVec;

				yield return null;
			}

			yield return new WaitForSeconds (2f);
			popupText.gameObject.SetActive (false);
			yield return new WaitForSeconds (0.5f);

			isPopupPublishing = false;
			break;

		}
	}

	public void GameOver()
	{
		if (playMode == Mode.IN_STAGE) {
			player.renderer.enabled = false;
			playMode = Mode.RESPAWNING;

			StartCoroutine ("Respawn", 3f);
			StartCoroutine ("Blink");
		} else if (playMode == Mode.LEVEL_WIN) {
			StartCoroutine ("Victory");
		}
	}

	IEnumerator Victory()
	{
		while (isPopupPublishing)
			yield return null;
		isPopupPublishing = true;
		StartCoroutine("PublishPopupReal", "Yayy!!\n\nYou Win!");

		while (isPopupPublishing)
			yield return null;
		Destroy (GameManager.instance.gameObject);
		Destroy (PickupManager.instance.gameObject);
		Application.LoadLevel (0);
	}

	IEnumerator Respawn(float delay)
	{
		player.transform.position = playerSpawnPos;
		//player.renderer.enabled = false;
		player.GetComponent<DamageScript> ().health = playerInitHealth * (stage + 1);

		while (isPopupPublishing)
			yield return null;
		isPopupPublishing = true;
		StartCoroutine("PublishPopupReal", "You Died!");

		while (isPopupPublishing)
			yield return null;

		isPopupPublishing = true;
		StartCoroutine("PublishPopupReal", "Respawning ...");

		while (isPopupPublishing)
			yield return null;
		playMode = Mode.IN_STAGE;
//		yield return new WaitForSeconds (delay);
//
//		StopCoroutine ("PublishPopupReal");


	}

	IEnumerator Blink()
	{
		while (playMode == Mode.RESPAWNING) {
			player.renderer.enabled = false;
			yield return new WaitForSeconds (0.5f);
			player.renderer.enabled = true;
			yield return new WaitForSeconds (0.5f);
		}



	}



	public void UpdateScore(int score)
	{
		playerScore += score;
	}
}
