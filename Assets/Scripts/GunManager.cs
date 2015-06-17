using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour {

	public float fireInterval = 0.1f;

	public float speed = 5f;
	public int damage = 1;

	private enum Bullet{NORMAL, CORROSIVE, SHOCKER, FIERY, CHILLER}
	private Bullet type = Bullet.NORMAL;		//0 - Normal ; 1 - Corrosion ; 2 - Electricity ; 3 - Fire ; 4 - Freeze
	public int GunType{
		get{ return (int)type; }
		set{ type = (Bullet)value; }
	}

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

		bulletCreates = myTransform.GetComponentsInChildren<BulletCreate>(true);		

		noOfGuns = myTransform.childCount;


	}

	//!!! CoRoutines are being stopped in the Update Loop. 
	//They should probably be stopped when Activating the new gun to not cause any issues through change in Position.

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

	public void ActivatePlayerGun()
	{
		int tempGunLevel = gunLevel;

		//Loop from BulletCreates[1-n] as the first gun (BulletCreate[0]) should always be active.
		for (int i = 1; i<noOfGuns && (tempGunLevel & 1) == 1; ++i) 
		{

			if(i == 1 && !bulletCreates[1].gameObject.activeSelf) 		//Change Gun Position
			{
				myGameObject.transform.localPosition = myGameObject.transform.localPosition + (new Vector3(-1f,0f,0f));
			}

				bulletCreates[i].gameObject.SetActive(true);
				tempGunLevel = tempGunLevel>>1;
		}

	}


}
