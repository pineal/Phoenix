using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public int level = 0;

	public int checkpointsPassed = 0;

	// Use this for initialization even if game has not been set up yet, or this script is not enabled.
	void Awake() {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);


	}

	//Use this for Initialization of variables or states only when this script is enabled, and before the first Update is called.
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
