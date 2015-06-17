using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {

	private Transform myTransform;

	private int bulletType = -1;
	private int damage = 1;
	private Transform left;
	private Transform right;
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

	private float topOfScreen=0.0f;

	// Use this for initialization
	void Start () {

		myTransform = transform;

		topOfScreen = Camera.main.orthographicSize;

		if(bulletType < 0)		//In case no bullet Type was set.
			bulletType = 1;	//strait line
		//bulletType = 2;	//Sine waveform
		//bulletType = 3;	//45, 90, 135 degree
		//left  = Instantiate(transform, transform.position, Quaternion.identity) as Transform ;
		//right  = Instantiate(transform, transform.position, Quaternion.identity) as Transform ;
	}
	
	// Update is called once per frame
	void Update () {

		if (bulletType == 1) {
			myTransform.position += speed * Time.deltaTime * transform.up;
		} else if (bulletType == 2) {
			float frequency = 1.0f;
			int amplitude = 1;
			transform.Translate (amplitude * Mathf.Sin (2 * Mathf.PI * frequency * Time.time) - amplitude * Mathf.Sin (2 * Mathf.PI * frequency * (Time.time - Time.deltaTime)), speed * Time.deltaTime, 0);
		} else if (bulletType == 3) {

			for (int i = 3; i < 60; i++) {				
				Instantiate(transform, transform.position, Quaternion.identity);
			}	

			transform.Translate (-speed * Time.deltaTime, speed * Time.deltaTime, 0);

			left.Translate (speed * Time.deltaTime, speed * Time.deltaTime, 0);

			//right.Translate (-speed * Time.deltaTime, speed * Time.deltaTime, 0);
		}

		if (myTransform.position.y >= topOfScreen) {
			this.gameObject.GetComponent<BulletDestroy>().Destroy();
		}

	}
}
