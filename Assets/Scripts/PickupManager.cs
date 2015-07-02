using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

	public static PickupManager instance = null;
	public int level = 0;
	public int stage = 0;
	
	public GameObject canvas;			//!!! Probably need to get rid of this
	public Text tempHealth;
	public GameObject pauseUI;			//!!! Probably need to get rid of this
	
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

	private Vector3 originPosition;
	private Quaternion originRotation;
	private float shake_decay;
	private float shake_intensity;
	private Transform CamTransform;

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

		//For Camera-Shake
		CamTransform = Camera.main.transform;
		originPosition = CamTransform.position;
		originRotation = CamTransform.rotation;
	}

	//!!! Delete/Manage this variable later
	private float oldHealth = 0;
	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.

	void Start () {

		tempHealth.text = playerDamageScript.Health.ToString();
		oldHealth = playerDamageScript.Health;
		
	}
	
	void Update()
	{
		if (Application.loadedLevel == 0)
			return;
		else if (player == null && GameManager.instance != null) {
			player = GameManager.instance.Player;
			return;
		}

		float newHealth = playerDamageScript.Health;

		if (newHealth != oldHealth) {
			tempHealth.text = playerDamageScript.Health.ToString();
			oldHealth = playerDamageScript.Health;
		}
	}

	public bool UpdateHealth(int health)		//Only for Player: The one in DamageScript is generic
	{
		playerDamageScript.AddHealth ((float)health);

		return true;
	}

	public void spawnPickup(Vector3 pos)
	{
		int index = Random.Range (-2, Pickup.Count+1);
		if (index >= 0)
			Instantiate (Pickup [index % Pickup.Count], pos, Quaternion.identity);
	}

	public void KaBoom(int damage){

		//	player = GameObject.Find("Enemy");
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag("EnemyAir");
		foreach (GameObject enemy in Enemies){
			enemy.GetComponent<DamageScript> ().TakeHealthDamage(damage);
		}
		GameObject Boss;
		if (Boss = GameObject.FindGameObjectWithTag("BigBoss")){
			Boss.GetComponent<DamageScript> ().TakeHealthDamage(damage);
		}

		//Camera Shake?
		StopCoroutine("Shake");
		SetShakeParam ();
		StartCoroutine ("Shake", 2f);
	}

	IEnumerator Shake (float counter){

		if (CamTransform == null) {
			Debug.LogError("CamTransform should not be null");
			yield break;
		}

		while (shake_intensity > 0 /*&& counter > 0*/){
			CamTransform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			CamTransform.rotation = new Quaternion(
				originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);
			shake_intensity -= shake_decay;

			//counter -= Time.deltaTime;
			yield return null;
		}

		CamTransform.position = originPosition;
		CamTransform.rotation = originRotation;
	}

	void ResetCamera()
	{
		if (CamTransform != null) {
			CamTransform.position = originPosition;
			CamTransform.rotation = originRotation;
		} else {
			Debug.LogError("CamTransform should not be null");
		}
	}

	void SetShakeParam(){
		ResetCamera ();

		shake_intensity = 0.05f;
		shake_decay = 0.0008f;
	}

}
