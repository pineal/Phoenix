using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public double kHook = 10.0;
	public double kDamp = 50.0;

	private float shipBoundaryX = 0.5f;
	private float shipBoundaryY = 0.7f;

	public Text debugText;

	private int score = 0;

	public int Score{
		get{ return score; }
		set{ score = value; }
	}


	// Use this for initialization
	void Start () {
	
		score = GameManager.instance.PlayerScore;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	void Update () {

		if (Input.GetButton ("Fire1")) {
			//Debug.Log ("fire button");

			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
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




		} 

		setDebugText ();
	}

	private Vector3 oldVel, curVel;

	void setDebugText()
	{
		if (debugText == null)
			return;

		Vector3 force;

		curVel = rigidbody2D.velocity;

		force = rigidbody2D.mass * (curVel - oldVel / Time.deltaTime);

		oldVel = curVel;

		debugText.text = "PlayerScript" + "\n" +
			"Position: " + 	transform.position.x.ToString("#.00") + ", " + 
							transform.position.y.ToString("#.00") + ", " + 
							transform.position.z.ToString("#.00") + "\n" +
			"Force: " + 	force.x.ToString("#.00") + ", " + 
							force.y.ToString("#.00") + ", " + 
							force.z.ToString("#.00") + "\n" +
			"Velocity: " + 	curVel.x.ToString("#.00") + ", " + 
							curVel.y.ToString("#.00") + ", " + 
							curVel.z.ToString("#.00") + "\n" +
			"Score: " + 	score					 + "\n" +

						"";
	}


	private void OnDisable()
	{
		GameManager.instance.PlayerScore = score;
	}
}
