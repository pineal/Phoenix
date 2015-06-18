using UnityEngine;
using System.Collections;

public class OnPickup : MonoBehaviour {


	public int healthBonus=10;

	private GameObject player;
	private Transform myTransform;
	private float origScale = 0.0f;

	private float rate = 0.2f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5f);
		myTransform = transform;
		origScale = myTransform.localScale.x;
	}

	private bool shrink = true;


	void Update()
	{
		Vector3 scale = myTransform.localScale;

		if (shrink) {
			scale.x -= Time.deltaTime * rate;
			if (scale.x < 0f)
				shrink = false;
		} else {
			scale.x += Time.deltaTime * rate;
			if (scale.x > origScale)
				shrink = true;
		}

		rate += (Time.deltaTime * 0.1f);
		myTransform.localScale = scale;
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag.Substring (0, 2) == "Pl" && collider.tag.Length == 6) {
			PickupManager.instance.UpdateHealth(healthBonus);
			Destroy (gameObject);
		}
	}
}
