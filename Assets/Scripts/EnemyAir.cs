using UnityEngine;
using System.Collections;

public class EnemyAir : MonoBehaviour {

	Transform player;
	private BulletCreate bulletCreateObj;

	private GameManager gameMgr = null;
	private int level;

	private bool onBoard = false;

	private float topOfScreen = 0f;
	private float rightOfScreen = 0f;
	private float spawnX = 0;

	private Transform myTransform;

	//For Rotate
	private bool isRotate = false;
	private int thresholdHealth = 0;
	private DamageScript damSc;

	//For Move
	private int moveType = 0;
		public int MoveType{
			get{ return moveType; }
			set{ moveType = value; }
		}
	private bool flag = false;
	Vector3 left, right;

	void Start()
	{
		bulletCreateObj = GetComponentInChildren<BulletCreate> ();

		gameMgr = GameManager.instance;
		if (gameMgr) {
			level = gameMgr.level;
		}

		myTransform = transform;
		spawnX = myTransform.position.x;

		topOfScreen = Camera.main.orthographicSize;
		rightOfScreen = Camera.main.orthographicSize * ( (float)Screen.width/(float)Screen.height );

		//For Rotate
		damSc = GetComponent<DamageScript> ();
		thresholdHealth = (int)(0.6f * damSc.health);

		//For Move
		left = new Vector3(-rightOfScreen+0.75f, topOfScreen-1.5f, 4f );
		right = new Vector3(rightOfScreen-0.75f, topOfScreen-1.5f, 4f );

		if (myTransform.position.y > topOfScreen)
			onBoard = false;
	}

	// Update is called once per frame
	void Update () {

		//Check if player exists
		if (player == null) {
			GameObject go = GameObject.Find("Player");

			if (go != null)
			{
				player = go.transform;
			}
		}

		if (player == null || !player.gameObject.activeSelf) {
			if(bulletCreateObj != null)
				bulletCreateObj.enabled = false;

			if (player == null)
				return;
		} 
		else {
			if(bulletCreateObj != null)
			bulletCreateObj.enabled = true;
		}

		
		//Player exists
		//Face in direction opposing the player
		if (Vector3.Dot (transform.rotation.eulerAngles, player.rotation.eulerAngles) != -1f) {

			//transform.rotation = Quaternion.Euler(0, 0, 180);
			Quaternion desiredRot = Quaternion.Euler(0, 0, 180);
			transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRot, 45f*Time.deltaTime);

		}

		if (myTransform.position.x > rightOfScreen || myTransform.position.x < -rightOfScreen ||
			myTransform.position.y > topOfScreen || myTransform.position.y < -topOfScreen) 
		{
			onBoard = false;
		}

		if (!onBoard) {
			myTransform.position = Vector3.MoveTowards(myTransform.position, 
			                                           new Vector3(spawnX, topOfScreen-1.5f, 4), 
			                                           3*Time.deltaTime);

			if ((topOfScreen - 1.5 - myTransform.position.y) <= 0.005f && 
			    (topOfScreen - 1.5 - myTransform.position.y) >= -0.005f )
			{
				onBoard = true;
			}
		}

		if(isRotate)
			Rotate ();

		if(onBoard)
			Move ();

	}

	public void CheckRotate()
	{
		if (damSc.health <= thresholdHealth && !isRotate) {

			isRotate = (Random.Range(0,10) <= 6) ? true : false;		//30% Chance to Rotate
		}
	}

	void Rotate()
	{
		myTransform.Rotate (Vector3.forward * (-180 * Time.deltaTime), Space.World);
	}

	void Move()
	{
		//Deal with Level specific Movements.
		switch (moveType) {

		case 0:

			if (flag)
			{	//Move Right
				if( (myTransform.position.x - right.x) >= -0.005f && (myTransform.position.x - right.x) <= 0.005f )
				{
					flag = false;
					float vertical = (Random.Range(0, 2) > 0) ? topOfScreen-1.5f : topOfScreen-3.5f;
					left.y = vertical;
				}
				else
				{
					myTransform.position = Vector3.MoveTowards(myTransform.position, right, 2*Time.deltaTime);
				}
			}
			else
			{	//Move Left
				if( (myTransform.position.x - left.x) >= -0.005f && (myTransform.position.x - left.x) <= 0.005f )
				{
					flag = true;
					float vertical = (Random.Range(0, 2) > 0) ? topOfScreen-1.5f : topOfScreen-3.5f;
					right.y = vertical;
				}
				else
				{
					myTransform.position = Vector3.MoveTowards(myTransform.position, left, 2*Time.deltaTime);
				}
			}
			
			break;
		}

	}
}
