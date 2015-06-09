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

	void OnDisable()
	{
		CancelInvoke();
	}
}
