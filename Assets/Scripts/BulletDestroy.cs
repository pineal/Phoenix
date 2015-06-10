using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

	void OnEnable()
	{
		Invoke ("Destroy", 2.5f);
	}

	void Destroy()
	{
		gameObject.SetActive (false);
	}

//	void Destroy()
//	{
//		Destroy (gameObject, 2f);
//	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log ("Collider: " + coll.name);
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}
