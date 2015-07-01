using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {

	private GameObject player;

	public GameObject DialogTitle;
	private Text title;
	public GameObject DialogText;
	private Text text;
	public GameObject DialogBox;
	private RectTransform dlgBoxRT;

	private Vector2 activePosition = new Vector2(0f,0f);
	private Vector2 inactivePosition = new Vector2(0f,0f);

	public enum DialogBoxProcesses {ACTIVATING, DEACTIVATING, INACTIVE, ACTIVE};
	private DialogBoxProcesses process;
	public DialogBoxProcesses DialogBoxProcess{
		get{ return process; }
	}

	// Use this for initialization
	void Active () {
		dlgBoxRT = DialogBox.GetComponent<RectTransform> ();
		title = DialogTitle.GetComponent<Text> ();
		text = DialogText.GetComponent<Text> ();

		process = DialogBoxProcesses.ACTIVE;
		activePosition = dlgBoxRT.anchoredPosition;
		inactivePosition = activePosition;
		inactivePosition.x = -(dlgBoxRT.rect.width);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ResetDialogBox()
	{
		//set position to zero
		if (dlgBoxRT != null) {
			dlgBoxRT.anchoredPosition = inactivePosition;

			process = DialogBoxProcesses.INACTIVE;
		} else {
			Active();
			ResetDialogBox();
		}
	}

	public void ActivateDialogBox(string Body, string Title = "... Incoming Msg", float time=0.1f)
	{//!!! Might not need time here.
		if (process == DialogBoxProcesses.INACTIVE || process == DialogBoxProcesses.DEACTIVATING) {
			StopAllCoroutines();
			text.text = Body;
			title.text = Title;
			StartCoroutine("Activate", time);
		} else {
			Debug.LogError("ActivateDialogBox() reached while process is not in INACTIVE or DEACTIVATING");
		}
	}

	public void DeactivateDialogBox()
	{
		if (process == DialogBoxProcesses.ACTIVE || process == DialogBoxProcesses.ACTIVATING) {
			StopAllCoroutines();
			StartCoroutine("Deactivate");
		} else {
			Debug.LogError("DeactivateDialogBox() reached while process is not in ACTIVE or ACTIVATING");
		}
	}

	private IEnumerator Activate(float time)
	{
		process = DialogBoxProcesses.ACTIVATING;

		while (dlgBoxRT.anchoredPosition.x < (activePosition.x-0.2f)) {
			dlgBoxRT.anchoredPosition = Vector2.Lerp(dlgBoxRT.anchoredPosition, activePosition, Time.deltaTime*4);
			yield return null;
		}

		dlgBoxRT.anchoredPosition = activePosition;

		//!!!Time may not be needed here.
		process = DialogBoxProcesses.ACTIVE;

		yield break;

	}

	private IEnumerator Deactivate()
	{
		process = DialogBoxProcesses.DEACTIVATING;
		
		while (dlgBoxRT.anchoredPosition.x > (inactivePosition.x+0.2f)) {
			dlgBoxRT.anchoredPosition = Vector2.Lerp(dlgBoxRT.anchoredPosition, inactivePosition, Time.deltaTime*6);
			yield return null;
		}

		dlgBoxRT.anchoredPosition = inactivePosition;
		
		//!!!Time may not be needed here.
		process = DialogBoxProcesses.INACTIVE;
		
		yield break;
		
	}
}
