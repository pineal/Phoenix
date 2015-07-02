using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	private Color color;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		color = renderer.material.color;
		color.a = Mathf.Lerp (renderer.material.color.a, 0.2f, Time.deltaTime*1.2f);
		renderer.material.color = color;

		if (color.a < 0.25f)
			Destroy (gameObject);

	}
}
