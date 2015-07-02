using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatScript : MonoBehaviour {

	private GameObject player = null;

	private bool buttonClicked = false;

	public Slider Speed;
	public Slider Damage;
	public Slider FireRate;
	public Text notes;

	// Use this for initialization
	void Start () {
		InitPlayer();
	}

	private void InitPlayer()
	{
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (player == null) {
			InitPlayer ();
			return;
		}

	}
}
