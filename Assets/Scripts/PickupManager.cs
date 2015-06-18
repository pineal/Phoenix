using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

	public static PickupManager instance = null;
	public int level = 0;
	public int stage = 0;
	
	public GameObject canvas;
	public Text tempHealth;
	public GameObject pauseUI;
	
	private float topOfActiveScreen = 0.0f;
	private float rightOfActiveScreen = 0.0f;
	
	public float TopOfActiveScreen{
		get{ return topOfActiveScreen; }
	}
	public float RightOfActiveScreen{
		get{ return rightOfActiveScreen; }
	}
	
	public List<GameObject> Pickup;

	private GameObject player = null;
	private DamageScript playerDamageScript = null;


	
	// Use this for initialization even if game has not been set up yet, or this script is not enabled.
	void Awake() {
		
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);

		//Get Player
		player = GameObject.Find("Player");
		playerDamageScript = player.GetComponent<DamageScript> ();

		topOfActiveScreen = Camera.main.orthographicSize - 1.5f;
		rightOfActiveScreen = Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height);

		
	}

	//!!! Delete/Manage this variable later
	private int oldHealth = 0;
	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.
	void Start () {

		tempHealth.text = playerDamageScript.health.ToString();
		oldHealth = playerDamageScript.health;
		
	}


	void Update()
	{
		if (player == null)
			return;

		int newHealth = playerDamageScript.health;

		if (newHealth != oldHealth) {
			tempHealth.text = playerDamageScript.health.ToString();
			oldHealth = playerDamageScript.health;
		}
	}

	public bool UpdateHealth(int health)
	{
		playerDamageScript.health += health;
		tempHealth.text = playerDamageScript.health.ToString();

		return true;
	}

	public void spawnPickup(Vector3 pos)
	{
		int index = Random.Range (-1, 2/*Pickup.Count*/);

		if (index >= 0)
			Instantiate (Pickup [index % Pickup.Count], pos, Quaternion.identity);
	}

}
