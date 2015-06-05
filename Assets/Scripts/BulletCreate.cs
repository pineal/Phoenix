using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletCreate : MonoBehaviour {

	public float fireTime = 0.05f;

	// Use this for initialization
	void Start () {

		InvokeRepeating ("Shoot", fireTime, fireTime);
	
	}

	void Shoot()
	{
		GameObject obj = ObjectPooler.current.getPooledObject ();

		if (obj == null)
			return;

		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		obj.SetActive (true);
	}

//
//	// Update is called once per frame
//	void Update () {
//
//
//	
//	}
}
