using UnityEngine;
using System.Collections;

public class BackrgoundScrolling : MonoBehaviour {

	public float scrollSpeed;
	private Vector3 startPosition;
	
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 20.0f);
		transform.position = startPosition + Vector3.up * newPosition;
	}
}

