using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	private GameObject player = null;
	private PlayerScript playerScript = null;

	public float health = 10f;

	private float timeToBlink = 0f;
	private int deathPoints = 0;

	public int DeathPoints{
		get{ return deathPoints; }
		set{ deathPoints = value; }
	}

	private bool isPlayer = false;

	void Awake()
	{
		if (transform.name.Substring (0, 3) == "Ene") {		//!!! Modify for various Enemies
			deathPoints = 10;
			isPlayer = false;
		} else {
			isPlayer = true;
		}

		if (player == null) {
			player = GameObject.Find("Player");

			if (player != null)
				playerScript = player.GetComponent<PlayerScript>();
		}
	}


	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log ("Trigger Collider: "+collider.name);

		if (collider.name.Substring (0, 4) == "Bull") {
			int damage = collider.GetComponent<BulletMove>().Damage;
			health -= damage;

			if(playerScript != null)
			{
				playerScript.Score += damage;
			}
		} else {
			health -= 1f;
			if(playerScript != null)
			{
				playerScript.Score += 1;
			}
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
			Destroy (gameObject);
			GameManager.instance.EnemyDestroyed();
		}	
		else
			GameManager.instance.GameOver ();
	}
}
