using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour {

	public double kHook = 10.0;
	public double kDamp = 50.0;

	private float shipBoundaryX = 0.5f;
	private float shipBoundaryY = 0.7f;

	private float topOfActiveScreen = 0.0f;

	public Text debugText;
	public Text scoreText;

	public EventSystem eventSystem;

	public GameObject UI;

	private int score = 0;

	public int Score{
		get{ return score; }
		set{ score = value; }
	}

	private bool inMenu = false;
	private List<RectTransform> pauseButtons = new List<RectTransform>();

	private int playMode = 0;

	// Use this for initialization
	void Start () {
	
		score = GameManager.instance.PlayerScore;

		topOfActiveScreen = GameManager.instance.TopOfActiveScreen;

		if (UI != null) {
			pauseButtons.AddRange (UI.GetComponentsInChildren<RectTransform>());

			for (int i=0; i<pauseButtons.Count; ++i)
			{
				if ( (pauseButtons[i].tag.Length >= 4 && pauseButtons[i].tag.Substring(0, 4) != "UIPa" ) 
				    || (pauseButtons[i].tag.Length < 4) )
				{
					pauseButtons.RemoveAt(i);
					--i;
				}

			}
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	void Update () {

		playMode = (int)GameManager.instance.PlayMode;


		if (Input.GetButton ("Fire1") && playMode == 1) {
			//Debug.Log ("fire button");



			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			if(UI != null && UI.activeSelf && !CheckIfClickInRect(Input.mousePosition))
			{
				UI.SetActive (false);
				Time.timeScale = 1f;
			}

			if (topOfActiveScreen >= mousePos.y && !UI.activeSelf) {


				mousePos.y += 0.5f;


				//Vertical Movement Restrictions
				if (mousePos.y + shipBoundaryY > Camera.main.orthographicSize) {
					mousePos.y = Camera.main.orthographicSize - shipBoundaryY;
				}
				if (mousePos.y - shipBoundaryY < -Camera.main.orthographicSize) {
					mousePos.y = -Camera.main.orthographicSize + shipBoundaryY;
				}

				//Horizontal Movement Restrictions
				float screenRatio = (float)Screen.width / (float)Screen.height;
				float cameraWidth = Camera.main.orthographicSize * screenRatio;

				if (mousePos.x + shipBoundaryX > cameraWidth) {
					mousePos.x = cameraWidth - shipBoundaryX;
				}
				if (mousePos.x - shipBoundaryX < -cameraWidth) {
					mousePos.x = -cameraWidth + shipBoundaryX;
				}

				var myPos = transform.position;

				rigidbody2D.velocity.Set (0, 0);
				
				Vector3 elasticForce, dampingForce, vectorToPlayer;

				vectorToPlayer = myPos - mousePos;
				vectorToPlayer.z = 0;

				double dist = (vectorToPlayer).magnitude;	//!!! Try to get rid of this.

				elasticForce = (float)((-kHook)) * vectorToPlayer;

				Vector3 myVelocity = rigidbody2D.velocity;

				dampingForce = (float)((-kDamp) * Vector3.Dot ((myVelocity), vectorToPlayer) / dist) * (vectorToPlayer / (float)dist);


				rigidbody2D.AddForce (elasticForce + dampingForce);


			} else {
				//Check for Button
			}

		} else if (playMode == 1) {
			Time.timeScale = 0.1f;

			if (UI != null && !UI.activeSelf)
			{
				UI.SetActive (true);
				inMenu = true;
			}

		} else {
			Time.timeScale = 1f;
			if (UI != null && UI.activeSelf)
			{
				UI.SetActive(false);
				inMenu = false;
			}
		}


		//!!! Might need to find a better place for this.
		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		setDebugText ();
	}

	private Vector3 oldVel, curVel;

	void setDebugText()
	{
		if (debugText == null)
			return;
		else {
			Vector3 force;

			curVel = rigidbody2D.velocity;

			force = rigidbody2D.mass * (curVel - oldVel / Time.deltaTime);

			oldVel = curVel;

			debugText.text = "PlayerScript" + "\n" +
				"Position: " + transform.position.x.ToString ("#.00") + ", " + 
				transform.position.y.ToString ("#.00") + ", " + 
				transform.position.z.ToString ("#.00") + "\n" +
				"Force: " + force.x.ToString ("#.00") + ", " + 
				force.y.ToString ("#.00") + ", " + 
				force.z.ToString ("#.00") + "\n" +
				"Velocity: " + curVel.x.ToString ("#.00") + ", " + 
				curVel.y.ToString ("#.00") + ", " + 
				curVel.z.ToString ("#.00") + "\n" +
				"Score: " + score + "\n" +

				"";
		}

		if (scoreText == null)
			return;
		else {
			scoreText.text = score.ToString("D9");
		}

	}

	private bool CheckIfClickInRect(Vector3 mousePos)
	{
		Camera cam = Camera.main;

		for (int i=0; i<pauseButtons.Count; i++) {
			if(RectTransformUtility.RectangleContainsScreenPoint(pauseButtons[i], Input.mousePosition, Camera.main))
				return true;
		}
		return false;
	}

	private void OnDisable()
	{
		GameManager.instance.PlayerScore = score;
	}
}
