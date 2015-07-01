using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShieldBar : MonoBehaviour {

	private Vector3 startPos;
	private Vector3 endPos;

	private RectTransform castBar;
	private RectTransform maskBar;
	
	private DamageScript playerDamageScript = null;
	GameObject player;
	
	private float maxValue = 0f, curValue = 0f;
	public Text tempShield;

	// Use this for initialization
	void Start () {

		castBar = GetComponent<RectTransform> ();
		maskBar = transform.parent.GetComponent<RectTransform> ();
		player = GameObject.Find("Player");
		playerDamageScript = player.GetComponent<DamageScript> ();

		startPos = castBar.position;
		maxValue = playerDamageScript.MaxShield;
		curValue = maxValue;
	
		tempShield.text = maxValue.ToString();

	}
	
	// Update is called once per frame
	void Update () {

		curValue = playerDamageScript.Shield;
		tempShield.text = curValue.ToString();

		if (curValue > -1) {
			//lerp the bar
			Vector3 lerpVec = Vector3.Lerp (startPos, maskBar.position, ((maxValue - curValue) / maxValue));
			//having the lerping animation 
			castBar.position = Vector3.MoveTowards (castBar.position, lerpVec, Time.deltaTime * 2);
		}
	}
}
