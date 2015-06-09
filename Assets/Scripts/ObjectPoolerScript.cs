using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolerScript : MonoBehaviour {

	public static ObjectPoolerScript current;
	public GameObject pooledObject;
	public int pooledAmount = 20;

	public bool willGrow = true;

	List<GameObject> pooledObjects;

	void Awake()
	{
		current = this;
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
}
