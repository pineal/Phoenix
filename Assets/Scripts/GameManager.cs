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

	public int checkpointsPassed = 0;

	private int playerScore = 0;

	public int PlayerScore{
		get{ return playerScore; }
		set{ playerScore = value; }
	}

	private int spawnedEnemies = 0, destroyedEnemies = 0, needToSpawn = 0;
	private bool spawnBigBoss = false;

	private GameObject player = null;
	[HideInInspector]public GunManager playerGunMgr = null;

	private int gameStatus = 0;		//0 - inactive, 1 - active, 2 - end of game

	private List<Transform> spawnPoints = new List<Transform>();

	private bool isPopupPublishing = false;


	private enum Mode {NOT_IN_STAGE, IN_STAGE, LEVEL_WIN, LEVEL_LOSS};

	private Mode playMode;

	public int PlayMode {
		get{ return (int)playMode;}
	}

	// Use this for initialization even if game has not been set up yet, or this script is not enabled.
	void Awake() {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		//Get Player
		player = GameObject.Find("Player");
		playerGunMgr = player.GetComponentInChildren<GunManager> ();

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


		// GunManager, set gunlevel


		//Activate Game
		gameStatus = 1;

		//Reset Popup Text UI
		popupText.gameObject.SetActive(false);

		//Set Play Mode
		playMode = Mode.NOT_IN_STAGE;

		StartCoroutine("UpdateStage");
	}

	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.
	void Start () {



	}
//
//	void Update()
//	{
//		if (playMode != Mode.IN_STAGE && pauseUI.activeSelf) {
//					pauseUI.SetActive(false);	
//		}
//	}
//
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
					StartCoroutine("PublishPopupReal", "Welcome!");

					while (isPopupPublishing)
						yield return null;
					StartCoroutine("PublishPopupReal", "Hello!");

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
				}
			}
			goto case 1;

			case 1:		// Stage 1
			{
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

		yield return null;
	}

	void ResetStageVars()
	{
		needToSpawn = spawnedEnemies = destroyedEnemies = 0;
	}

	public void EnemyDestroyed()
	{
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

	public void PublishPopupUI(string text)
	{
		if (true) {

			//popupText.gameObject.SetActive(true);

			popupText.text = text;

			StartCoroutine("PublishPopupReal");
		}
	}

	IEnumerator PublishPopupReal( string text)
	{
		RectTransform rt = popupText.gameObject.GetComponent<RectTransform>();

		popupText.text = text; 

		while (true) {

			if (popupText.gameObject.activeSelf) {

				yield return null;
				continue;
			
			} else {

				isPopupPublishing = true;

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
