using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

	void onEnable()
	{
		Invoke ("Destroy", 1.5f);
	}

	void Destroy()
	{
		gameObject.SetActive (false);
	}

	void onDisable()
	{
		CancelInvoke();
	}
}
