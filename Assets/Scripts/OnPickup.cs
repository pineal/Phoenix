using UnityEngine;
using System.Collections;

public class OnPickup : MonoBehaviour {


	public int healthBonus=10;
	public int damage = 5;

	private GameObject player;
	private Transform myTransform;
	private float origScale = 0.0f;

	private float rate = 0.2f;

	string myTag;
	private GameObject myGameObject;
	private bool collectAll = false;

	// Use this for initialization
	void Start () {
		player = GameManager.instance.Player;
		myGameObject = gameObject;
		Invoke ("DestroyMe", 5f);
		myTransform = transform;
		origScale = myTransform.localScale.x;
		myTag = myGameObject.tag;
	}

	private bool shrink = true;

	void DestroyMe()
	{
		Destroy (myGameObject);
	}

	void Update()
	{
		int pickup=0;
		if (! int.TryParse(myTag.Substring(6), out pickup))
			return;

		//Only for Health Pickup
		if (pickup == 0 && !collectAll) {
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

		// For Pickups to move towards players.
		if (GameManager.instance.PlayMode == (int)GameManager.Mode.NOT_IN_STAGE && !collectAll)
		{
			CancelInvoke ("DestroyMe");
			collectAll = true;
			myTransform.localScale = new Vector3(origScale, myTransform.localScale.y, myTransform.localScale.z);

		}

		if (collectAll && player != null) {
			myTransform.position = Vector3.Lerp(myTransform.position, player.transform.position, 0.07f);
		}

	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag.Substring (0, 2) == "Pl" && collider.tag.Length == 6) {

			int pickup = 0;

			if (! int.TryParse(myTag.Substring(6), out pickup))
			    return;

			switch (pickup)
			{
			case 0:
			{
				PickupManager.instance.UpdateHealth(healthBonus);
			}
				break;
			case 1:
			{
				PickupManager.instance.KaBoom(damage);
			}
				break;
			}
			Destroy (gameObject);
		}
	}
}
