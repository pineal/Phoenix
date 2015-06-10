using UnityEngine;
using System.Collections;

public class EnemyAirMovement : MonoBehaviour {

	Transform player;
	private BulletCreate bulletCreateObj;

	private GameManager gameMgr = null;
	private int level;

	void Start()
	{
		bulletCreateObj = GetComponentInChildren<BulletCreate> ();

		gameMgr = GameManager.instance;
		if (gameMgr) {
			level = gameMgr.level;
		}
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

		if (player == null) {
			if(bulletCreateObj != null)
				bulletCreateObj.enabled = false;

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


		Move ();

	}

	void Move()
	{
		//Deal with Level specific Movements.
	}
}
