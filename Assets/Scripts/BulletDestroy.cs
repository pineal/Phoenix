using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

	void OnEnable()
	{
		Invoke ("Destroy", 2.5f);
	}

	public void Destroy()
	{
		gameObject.SetActive (false);
	}

//	void Destroy()
//	{
//		Destroy (gameObject, 2f);
//	}

	void OnTriggerEnter2D(Collider2D coll)
	{
//		Debug.Log ("Collider: " + coll.name);

		if ( coll.tag.Length > 12 && coll.tag.Substring(0,12) == "PlayerBullet")		//!!! Need to adjust this according to Bullet TYPE.
			return;

		if (coll.tag.Length > 4 && coll.tag.Substring (0, 4) == "Expl")
			return;

		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}
