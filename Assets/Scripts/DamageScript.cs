using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	private GameObject player = null;
	private PlayerScript playerScript = null;
	private EnemyAir enemyAirScript = null;
	private BossMove bossMoveScript = null;

	public int health = 10;

	private float timeToBlink = 0f;
	private int deathPoints = 0;

	public int DeathPoints{
		get{ return deathPoints; }
		set{ deathPoints = value; }
	}

	private bool isPlayer = false;
	private bool isBoss = false;

	private Transform myTransform;

	void Awake()
	{
		if (transform.tag.Substring (0, 3) == "Ene") {		//!!! Modify for various Enemies
			deathPoints = 10;
			isPlayer = false;
			enemyAirScript = GetComponent<EnemyAir>();
		} else if (transform.tag.Substring (0, 3) == "Big") {
			deathPoints = 50;
			isPlayer = false;
			isBoss = true;
			bossMoveScript = GetComponent<BossMove>();
		}
		else {
			isPlayer = true;
		}

		if (player == null) {
			player = GameObject.Find("Player");

			if (player != null)
				playerScript = player.GetComponent<PlayerScript>();
		}

		myTransform = transform;
	}


	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log ("Trigger Collider: "+collider.name);

		if (GameManager.instance.PlayMode != (int)GameManager.Mode.IN_STAGE)
			return;

		switch (isPlayer) {
		case false:
			if (collider.name.Substring (0, 4) == "Bull") {
				int damage = collider.GetComponent<BulletMove> ().Damage;
				health -= damage;

				if (playerScript != null) {
					playerScript.Score += damage;
				}
			} else {
				health -= 3;
				if (playerScript != null) {
					playerScript.Score += 1;
				}
			}
			if(enemyAirScript != null)
				enemyAirScript.CheckRotate();
			break;
		case true:
			if (collider.name.Substring (0, 4) == "Bull") {
				int damage = collider.GetComponent<BulletMove> ().Damage;
				health -= damage;
				
//				if (playerScript != null) {
//					playerScript.Score -= damage;
//				}
			} else if (collider.tag.Substring (0, 4) != "Pick") {
				health -= 7;
//				if (playerScript != null) {
//					playerScript.Score -= 5;
//				}
			}
			break;
		}



		timeToBlink = 2f;

		StopAllCoroutines();
		renderer.enabled = true;
		StartCoroutine ( Blink(timeToBlink) );

	}

	void Update() {

		if (health <= 0) {
			Die();
		}
	}

	IEnumerator Blink(float time) {

		renderer.enabled = false;
		yield return new WaitForSeconds (0.05f);
		renderer.enabled = true;
	}

	void Die()
	{
		if(playerScript != null)
		{
			playerScript.Score += deathPoints;
		}
		if (!isPlayer) {
			GameManager.instance.EnemyDestroyed(gameObject.tag, myTransform.position);
			Destroy (gameObject);
		}	
		else
			GameManager.instance.GameOver ();
	}
}
