using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDButtons : MonoBehaviour {

	public StatScript PrimaryStats;

	void Active()
	{
//		GameManager.instance.playerGunMgr.GunType
	}

	public void PrimaryWeaponChange()
	{
		GameManager.instance.playerGunMgr.GunType = (GameManager.instance.playerGunMgr.GunType + 1) % 2;

		PrimaryStats.ChangeButtonUI ();

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
