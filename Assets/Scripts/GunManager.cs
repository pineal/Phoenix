using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour {

	private float maxFireInterval = 0.055f;
	private float maxSpeed = 15f;
	private float maxDamage = 10f;
	public float MaxFireInterval{
		get{ return maxFireInterval; }
	}
	public float MaxSpeed{
		get{ return maxSpeed; }
	}
	public float MaxDamage{
		get{ return maxDamage; }
	}

	public float fireInterval = 0.1f;

	public float speed = 5f;
	public float damage = 1f;

	public enum Bullet{NORMAL, CORROSIVE, SHOCKER, FIERY, CHILLER}		//Do not change. If you do, keep in mind the impact it will have in BulletAnimation
	private Bullet type = Bullet.NORMAL;		//0 - Normal ; 1 - Corrosion ; 2 - Electricity ; 3 - Fire ; 4 - Freeze
	public int GunType{
		get{ return (int)type; }
		set{ type = (Bullet)value;
			setGunProp(); }
	}

	public float prob= 0.0f;

	public int burstAmt = 3;
	public float burstInterval = 0.5f;

	public int gunLevel = 128; //8th bit is to toggle between Change needed or not.
	public int noOfGuns = 0;

	private Transform myTransform; 
	private GameObject myGameObject;
	private bool isPlayer = false;

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

		if (transform.parent.gameObject.tag == "Player")
			isPlayer = true;

		if (isPlayer)
			setGunProp ();


	}

	void setGunProp()
	{
		switch (type) {
		case Bullet.NORMAL:
		{
			fireInterval = maxFireInterval * 5f;
			speed = maxSpeed * (2f/3f);
			damage = maxDamage * 0.1f;

//			fireInterval = MaxFireInterval;
//			speed = MaxSpeed;
//			damage = (int)MaxDamage;

			prob = 0f;
			burstAmt = 0;
			burstInterval = 0.5f;

			break;
		}
		case Bullet.CORROSIVE:
		{
			//fireInterval = maxFireInterval * 10f;
			fireInterval = maxFireInterval * 5f;
			speed = maxSpeed * 0.5f;
			damage = maxDamage * 0.4f;
			prob = 0f;
			burstAmt = 0;
			burstInterval = 0.5f;
			
			break;
		}
		case Bullet.FIERY:
		{
			fireInterval = maxFireInterval * 6f;
			speed = maxSpeed * 0.3f;
			damage = maxDamage * 0.2f;
			prob = 0f;
			burstAmt = 0;
			burstInterval = 0.5f;
			
			break;
		}
		case Bullet.SHOCKER:
		{
			fireInterval = maxFireInterval * 7f;
			speed = maxSpeed * 0.2f;
			damage = maxDamage * 0.25f;
			prob = 0f;
			burstAmt = 0;
			burstInterval = 0.5f;
			
			break;
		}
		case Bullet.CHILLER:
		{
			fireInterval = maxFireInterval * 6.5f;
			speed = maxSpeed * 0.3f;
			damage = maxDamage * 0.2f;
			prob = 0f;
			burstAmt = 0;
			burstInterval = 0.5f;
			
			break;
		}
		default:
		{
			fireInterval = maxFireInterval * 5f;
			speed = maxSpeed * (2f/3f);
			damage = maxDamage * 0.1f;
			prob = 0f;
			burstAmt = 0;
			burstInterval = 0.5f;
			
			break;
		}
		}

		//Set Properties for each gun
		for (int i=0; i<noOfGuns; ++i) {
			bulletCreates[i].fireTime = fireInterval;
			bulletCreates[i].BurstAmt = burstAmt;
			bulletCreates[i].BurstInterval = burstInterval;
		}
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
