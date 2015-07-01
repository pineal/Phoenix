using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarAlternate : MonoBehaviour {

	public DamageScript playerDmgScript;

	private Image myImage=null;

	// Use this for initialization
	void Start () {
	
		myImage = this.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (playerDmgScript != null && myImage.fillMethod == Image.FillMethod.Horizontal) {
			myImage.fillAmount = Mathf.Lerp(myImage.fillAmount, ((float)playerDmgScript.Health/(float)playerDmgScript.MaxHealth), Time.deltaTime*3);
		}
	
	}
}
