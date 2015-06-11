using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {

	private int damage = 1;
	public int Damage{ 
		get{ return damage; } 
		set{ damage = value; }
	}

	public float speed = 5f;
	public float Speed{ 
		get{ return speed; } 
		set{ speed = value; }
	}

	private int type = 0;
	public int Type{ 
		get{ return type; } 
		set{ type = value; }
	}

	private float prob = 0f;
	public float ProbabilityOfDamage{ 
		get{ return prob; } 
		set{ prob = value; }
	}
	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, speed * Time.deltaTime, 0);
	
	}
}
