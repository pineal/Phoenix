using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour {

	public float fireInterval = 0.1f;

	public float speed = 5f;
	public int damage = 1;
	public int type = 0;		//0 - Normal ; 1 - Corrosion ; 2 - Electricity ; 3 - Fire
	public float prob= 0.0f;

	public int burstAmt = 3;
	public float burstInterval = 0.5f;

	public int gunLevel = 128; //8th bit is to toggle between Change needed or not.
	public int noOfGuns = 0;

	private Transform myTransform; 
	private GameObject myGameObject;

	BulletCreate[] bulletCreates;

	void Awake()
	{
		myTransform = transform;
		myGameObject = gameObject;
	}

	void Start()
	{

		bulletCreates = myTransform.GetComponentsInChildren<BulletCreate>(true);		//bulletCreates[0] is the Assembly itself.

		noOfGuns = myTransform.childCount;


	}

	void Update()
	{
		if (((gunLevel & 128) | 1) != 1) 
		{
			for (int i=0; i<noOfGuns; ++i)
			{
				BulletCreate obj = bulletCreates[i];

				if (obj.gameObject.activeInHierarchy)
				{	
					obj.StopAllCoroutines();
					obj.StartCoroutine( obj.Shoot(fireInterval*10f, burstInterval) );
				}
			}

			gunLevel = gunLevel & ~(128);
		}

	}


}
