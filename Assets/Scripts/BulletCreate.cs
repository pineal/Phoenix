using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletCreate : MonoBehaviour {

	public float fireTime = 0.1f;

	public ObjectPoolerScript poolerScript;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;


	public int test=0;

	//ObjectPoolerScript objP;

	// Use this for initialization
	void Start () {

		poolerScript.pooledObject = pooledObject;
		poolerScript.pooledAmount = pooledAmount;
		poolerScript.willGrow = willGrow;
		poolerScript.start ();

		InvokeRepeating ("Shoot", fireTime, fireTime);

	}

	void Shoot()
	{
		//GameObject obj = ObjectPoolerScript.current.getPooledObject ();

		GameObject obj = poolerScript.getPooledObject ();

		if (obj == null)
			return;

		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		obj.SetActive (true);
	}


//	public float fireTime = 0.25f;
//	public GameObject bullet;
//
//	void Start()
//	{
//		InvokeRepeating ("Fire", 2f, fireTime);
//
//	}
//
//	void Fire()
//	{
//		Debug.Log("Fire");
//		Instantiate (bullet, transform.position, Quaternion.identity);
//	}

}
