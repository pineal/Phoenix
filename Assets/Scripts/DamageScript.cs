using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DamageScript : MonoBehaviour {

	private GameObject player = null;
	private PlayerScript playerScript = null;		
	private EnemyAir enemyAirScript = null;			
	private BossMove bossMoveScript = null;			


	private float maxHealth = 0;
	public float MaxHealth{
		get{ return maxHealth; }
	}
	private float health = 20f;
	public float Health{
		get{ return health; }
		set{ health = value; }
	}

	//Shield
	private float maxShield = 10f;
	public float MaxShield{
		get{ return maxShield; }
	}
	private float shield = 5f;
	public float Shield{
		get{ return shield; }
		set{ shield = value; }
	}

	private float shieldDelay = 3f;
	private float shieldRechargeRate = 0.8f;
	public float ShieldRechargeRate {
		get{ return shieldRechargeRate; }
	}
	public float ShieldDelay {
		get{ return shieldDelay; }
	}

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
			shield = 0;
		} else if (transform.tag.Substring (0, 3) == "Big") {
			deathPoints = 50;
			isPlayer = false;
			isBoss = true;
			bossMoveScript = GetComponent<BossMove>();
			shield = 0;
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

		maxHealth = (float)health;
		maxShield = shield;
	}


	void OnTriggerEnter2D(Collider2D collider)
	{
		//Debug.Log ("Trigger Collider: "+collider.name);

		if (GameManager.instance.PlayMode != (int)GameManager.Mode.IN_STAGE)
			return;

		switch (isPlayer) {
		case false:
			if (collider.name.Substring (0, 4) == "Bull") {
				int damage = collider.GetComponent<BulletMove> ().Damage;

				if(shield > 0)
				{
					shield -= damage;
					StopCoroutine("ShieldRecharge");
					StartCoroutine("ShieldRecharge");
				}
				else
					health -= damage;

				if (playerScript != null) {
					playerScript.Score += damage;
				}
			} else {
				if (shield > 0)
					shield -= 3;
				else
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

				if(shield > 0)		//Shield Exists
				{
					shield -= damage;
					StopCoroutine("ShieldRecharge");
					StartCoroutine("ShieldRecharge");

				}
				else 				//Shield is Absent
				{
					health  -= damage;
				}

				
				if(shield < 0) //Incase damage was more than shield capacity this frame;
				{
					health += shield;
					shield = 0;
				}

			} else if (collider.tag.Substring (0, 4) != "Pick") {
				if(shield > 0)		//Shield exists
				{
					shield -= 7;
					StopCoroutine("ShieldRecharge");
					StartCoroutine("ShieldRecharge");

				}
				else 				//Shield is absent
				{
					health -= 7;
				}

				if (shield < 0)		//Incase damage was more than shield capacity this frame;
				{
					health += shield;
					shield = 0;
				}
//				if (playerScript != null) {
//					playerScript.Score -= 5;
//				}
			}
			break;
		}
			
		timeToBlink = 2f;

		StopCoroutine("Blink");
		renderer.enabled = true;
		StartCoroutine ( "Blink", timeToBlink );

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

	IEnumerator ShieldRecharge()
	{
		yield return new WaitForSeconds (shieldDelay);

		while (shield < maxShield) {
			shield += shieldRechargeRate * Time.deltaTime;
			yield return null;
		}
		shield = maxShield;
		yield break;
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
