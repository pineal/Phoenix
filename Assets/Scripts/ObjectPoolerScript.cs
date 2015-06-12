using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolerScript : MonoBehaviour {

	//public static ObjectPoolerScript current;

	private GameObject pooledObject;
	public GameObject PooledObject{ 

		get
		{
			return pooledObject;
		}

		set
		{
			pooledObject = value;
		}
	}


	private int pooledAmount = 20;
	public int PooledAmount {
		get;
		set;
	}

	public bool willGrow = true;

	List<GameObject> pooledObjects;

	void Awake()
	{
		//current = this;
	}

	// Use this for initialization
	public void start () {
	
		pooledObjects = new List<GameObject> ();

		for (int i=0; i<pooledAmount; ++i) {

			GameObject obj = (GameObject)Instantiate(pooledObject);

			obj.SetActive(false);
			pooledObjects.Add(obj);
		}

	}
	
	public GameObject getPooledObject()
	{
		for (int i=0; i<pooledObjects.Count; ++i) {

			if(!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}

		if (willGrow) {
			GameObject obj = (GameObject) Instantiate(pooledObject);
			pooledObjects.Add (obj);
			return obj;
		}

		return null;
	}

	public void killObjects()
	{
		for (int i=0; i<pooledObjects.Count; ++i) {
			
			Destroy(pooledObjects[i], 2f);
		}
	}

}
