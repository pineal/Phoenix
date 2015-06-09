using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	public float health = 1f;

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log ("Trigger Collider: "+collider.name);

//		GameObject go = GameObject.Find (collider.name);
//
//		go.

		health -= 1f;

	}

	void Update()
	{
		if (health <= 0) {
			Die();
		}
	}

	void Die()
	{
		Destroy (gameObject);
	}
}
