using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatScript : MonoBehaviour {

	private GameObject player = null;
	private GunManager playerGunMgr = null;

	private bool buttonClicked = false;

	public Slider Speed;
	public Slider Damage;
	public Slider FireRate;
	public Text notes;

	public Sprite NormalBullet;
	public Sprite FieryBullet;
	public Sprite CorrosiveBullet;
	public Sprite ShockerBullet;
	public Sprite ChillerBullet;

	public Image ButtonImage;

	// Use this for initialization
	void Start () {
		InitPlayer();

	}

	private void InitPlayer()
	{
		player = GameObject.Find ("Player");
		if (player != null)
			playerGunMgr = player.GetComponentInChildren<GunManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (player == null) {
			InitPlayer ();
			return;
		}

		if (Time.timeScale < 1f && playerGunMgr != null) {			//!!! TimeScale is <1 only when Menu pops up - Might need a better handling
			Speed.value 	= ((float)playerGunMgr.speed / playerGunMgr.MaxSpeed);
			Damage.value 	= ((float)playerGunMgr.damage / playerGunMgr.MaxDamage);
			FireRate.value 	= (playerGunMgr.MaxFireInterval / (float)playerGunMgr.fireInterval);		//Rate, hence inverse division

			if (playerGunMgr.burstAmt != 0 || playerGunMgr.prob > 0f)
			{
				if(notes.rectTransform.parent.gameObject.activeInHierarchy)
				{
					if(playerGunMgr.burstAmt != 0)
						notes.text = "- Burst Fire\n";

					if(playerGunMgr.prob != 0f)
						notes.text = notes.text + "- Elemental Damage";

				}

			}
			else{
				notes.rectTransform.parent.gameObject.SetActive(false);
			}
		}

	}

	public void ChangeButtonUI()
	{
		switch (playerGunMgr.GunType){
		case (int)GunManager.Bullet.NORMAL:
			ButtonImage.sprite = NormalBullet;
			break;
		case (int)GunManager.Bullet.FIERY:
			ButtonImage.sprite = FieryBullet;
			break;
		case (int)GunManager.Bullet.CORROSIVE:
			ButtonImage.sprite = CorrosiveBullet;
			break;
		case (int)GunManager.Bullet.SHOCKER:
			ButtonImage.sprite = ShockerBullet;
			break;
		case (int)GunManager.Bullet.CHILLER:
			ButtonImage.sprite = ChillerBullet;
			break;
		default:
			ButtonImage.sprite = NormalBullet;
			break;
		}

	}
}
