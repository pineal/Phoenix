using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject CanvasObj;

	private GameObject player = null;
	private Transform playerTransform = null;

	private DamageScript damScript = null;

	private GameObject myGameObject = null;
	private RectTransform myRectTransform = null;
	private Image myImage = null;

	private RectTransform CanvasRect = null;

	// Use this for initialization
	void Start () {

		myGameObject = gameObject;
		myRectTransform = myGameObject.GetComponent<RectTransform> ();
		myImage = myGameObject.GetComponent<Image> ();

		CanvasRect = CanvasObj.GetComponent<RectTransform> ();

		FindPlayer ();
	
	}

	void FindPlayer()
	{
		player = GameObject.Find ("Player");

		if (player != null) {
			playerTransform = player.transform;
			damScript = player.GetComponent<DamageScript> ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (player == null) {
			FindPlayer ();
		} else {

			float canvasW = CanvasRect.sizeDelta.x;
			float canvasH = CanvasRect.sizeDelta.y;

			Vector3 viewPortPos = Camera.main.WorldToViewportPoint(playerTransform.position + new Vector3(0f, 1f, 0f));

			myRectTransform.anchoredPosition = new Vector2 ( (viewPortPos.x*canvasW), (viewPortPos.y*canvasH));


			//*****************

//			if (myImage.fillAmount < 1f && (myImage.fillAmount + Time.deltaTime <= 1f) )
//			{
//				myImage.fillAmount += Time.deltaTime;
//
//			}

			if (myImage.type == Image.Type.Filled) {
				float finalRatio = (damScript.Shield<0) ? 0f:(float)damScript.Shield/((float)damScript.MaxShield);
				myImage.fillAmount = Mathf.Lerp(myImage.fillAmount, finalRatio, Time.deltaTime*10);
			}



		}
	
	}
}
