using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	public float health = 10f;

	private float timeToBlink = 0f;

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log ("Trigger Collider: "+collider.name);

		health -= 1f;

		timeToBlink = 2f;

		StopCoroutine ("Blink");
		renderer.enabled = true;
		StartCoroutine ("Blink", timeToBlink);

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
		Destroy (gameObject);
	}
}
