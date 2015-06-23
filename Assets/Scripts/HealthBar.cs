using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	private Vector3 startPos;
	private Vector3 endPos;

	private RectTransform castBar;
	private RectTransform maskBar;
	
	private DamageScript playerDamageScript = null;
	GameObject player;
	
	private float maxValue = 0f, curValue = 0f;
	
	// Use this for initialization
	void Start () {

		castBar = GetComponent<RectTransform> ();
		maskBar = transform.parent.GetComponent<RectTransform> ();
		player = GameObject.Find("Player");
		playerDamageScript = player.GetComponent<DamageScript> ();

		startPos = castBar.position;
		maxValue = playerDamageScript.health;
		curValue = maxValue;

	}
	
	// Update is called once per frame
	void Update () {
		curValue = playerDamageScript.health;
		//lerp the bar
		Vector3 lerpVec = Vector3.Lerp (startPos, maskBar.position, ((maxValue - curValue) / maxValue));
		//having the lerping animation 
		castBar.position = Vector3.MoveTowards (castBar.position, lerpVec, Time.deltaTime * 2);

	}
}
