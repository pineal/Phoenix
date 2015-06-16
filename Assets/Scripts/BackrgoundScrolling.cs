using UnityEngine;
using System.Collections;

public class BackrgoundScrolling : MonoBehaviour {

	public float scrollSpeed;
	public Vector3 startPosition;
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {

		myTransform = transform;
		startPosition = myTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 20.0f);
		myTransform.position = startPosition + Vector3.up * newPosition;
	}
}

