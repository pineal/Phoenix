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

	private GameObject objParent = null;
	private GunManager gunMgr = null;

	private string parentTag;

	private int burstAmt = 0;
	private float burstInterval =0f;




	//ObjectPoolerScript objP;

	// Use this for initialization
	void Start () {

		gunMgr = GetComponentInParent<GunManager> ();

		parentTag = transform.parent.tag;

		poolerScript.PooledObject = pooledObject;
		poolerScript.PooledAmount = pooledAmount;
		poolerScript.willGrow = willGrow;
		poolerScript.start ();

		fireTime = gunMgr.fireInterval;

		burstAmt = gunMgr.burstAmt;
		burstInterval = gunMgr.burstInterval;

//		if (parentTag [0] == 'E') { 	//Enemy
//			InvokeRepeating ("Shoot", fireTime * 10f, fireTime * 5f);
//		} else {						//Player
//			InvokeRepeating ("Shoot", fireTime * 10f, fireTime);
//		}

		//InvokeRepeating ("Shoot", fireTime * 10f, fireTime);

		//StartCoroutine ( Shoot(fireTime * 10f, burstInterval) );

	}

	//void ShootReal()
	public IEnumerator Shoot(float initWait, float burstInt)
	{
		//GameObject obj = ObjectPoolerScript.current.getPooledObject ();
		yield return new WaitForSeconds (initWait);

		while (true) {

			if (!this.enabled)
			{
				yield return null;
				continue;
			}

			for(int i=0; i<burstAmt || burstAmt==0; ++i)
			{
				GameObject obj = poolerScript.getPooledObject ();

				BulletMove moveObj = obj.GetComponent<BulletMove> ();

				//if (parentTag[0] == 'E')
				{
					moveObj.Damage = gunMgr.damage;
					moveObj.Speed = gunMgr.speed;
					moveObj.Type = gunMgr.type;
					moveObj.ProbabilityOfDamage = gunMgr.prob;
				}

				if (obj == null)
				{
					yield return null;
					continue;
				}

				if (objParent == null)
					objParent = transform.parent.gameObject;

				obj.layer = objParent.layer;

				obj.transform.position = transform.position;
				obj.transform.rotation = transform.rotation;
				obj.SetActive (true);

				Animator animator = obj.GetComponent<Animator>();

				animator.SetInteger("bulletType", gunMgr.type);

				yield return new WaitForSeconds(fireTime);
			}
			yield return new WaitForSeconds(burstInterval);
		}
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

	void OnDestroy()
	{
		if(poolerScript != null)
			poolerScript.killObjects ();
	}

}
