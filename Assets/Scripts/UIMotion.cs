using UnityEngine;
using System.Collections;

public class UIMotion : MonoBehaviour {

	private Vector2 myInitPos;
	private RectTransform myRT;
	private GameObject player = null;
	private RectTransform UI2 = null;

	void Awake()
	{
		myRT = GetComponent<RectTransform> ();
		myInitPos = myRT.anchoredPosition;
		player = GameObject.Find ("Player");
		UI2 = transform.parent.gameObject.GetComponent<RectTransform> ();
	}

	void OnEnable()
	{
		if (player != null) {
			float canvasW = UI2.sizeDelta.x;
			float canvasH = UI2.sizeDelta.y;
			
			Vector3 viewPortPos = Camera.main.WorldToViewportPoint(player.transform.position);

			viewPortPos.x -= myRT.anchorMin.x;
			viewPortPos.y -= myRT.anchorMin.y;
			
			myRT.anchoredPosition = new Vector2 ( (viewPortPos.x*canvasW), (viewPortPos.y*canvasH));
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isActiveAndEnabled) {
			myRT.anchoredPosition = Vector2.Lerp (myRT.anchoredPosition, myInitPos, 0.1f);
		}
	
	}
}
