using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	public float fireInterval = 0.1f;

	public float speed = 5f;
	public float damage = 1f;
	public int type = 0;		//0 - Normal ; 1 - Corrosion ; 2 - Electricity ; 3 - Fire
	public float prob= 0.0f;

	public int burstAmt = 3;
	public float burstInterval = 0.5f;


}
