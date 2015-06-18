using UnityEngine;
using System.Collections;

public class HUDButtons : MonoBehaviour {

	public void PrimaryWeaponChange()
	{
		GameManager.instance.playerGunMgr.GunType = (GameManager.instance.playerGunMgr.GunType + 1) % 2;
	}

	public void LoadLevel (int level)
	{
		Application.LoadLevel (level);
	}

	public void Quit(int level)
	{
		Application.Quit ();
	}
}
