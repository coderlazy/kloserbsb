using UnityEngine;
using System.Collections;

public class GameController : MonoSingleton<GameController> 
{
	public GameObject m_EndObj;
	public UILabel m_BounceLb;
	public AudioClip[] m_AudioBounce;
	public AudioClip m_AudioCrush;
	public UILabel lbTest;
	private static bool rated = false;
	public GameObject buttonSkipAdmod;
	public GameState getState
	{
		get { return gameState; }
	}

	private GameState gameState;
	private Collider2D m_Collider;
	private AudioSource m_Audio;

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 60;

		gameState = GameState.Begin;

		m_Collider = GetComponent<Collider2D>();
		m_Audio = GetComponent<AudioSource>();

		m_BounceLb.text = string.Empty + ModelLevel.CurrentLevel.maxBounce;
	}

	void Update()
	{
		if (gameState == GameState.Running)
		{
			GameValue.GameTime += Time.deltaTime;

			if (GameValue.CrystalRemain == 0)
			{
				StartCoroutine(Win());
			}
			else if (GameValue.BounceRemain < 0)
			{
				StartCoroutine(Lose());
			}
		}
	}

	IEnumerator Win()
	{
		gameState = GameState.Win;

		GameValue.Ball.gameObject.SetActive(false);

		Resources.UnloadUnusedAssets();
		System.GC.Collect();

		yield return new WaitForSeconds(0.75f);

		m_Collider.enabled = false;

		if (GameValue.GameTime <= ModelLevel.CurrentLevel.milestones.x)
			GameValue.GameStar = 3;
		else if (GameValue.GameTime <= ModelLevel.CurrentLevel.milestones.y)
			GameValue.GameStar = 2;
		else
			GameValue.GameStar = 1;
		m_EndObj.SetActive(true);
#if UNITY_ANDROID
		//AndroidMessage.Create("tat quang cao: ",AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased (GPaymnetManagerExample.ANDROID_TEST_PURCHASED)+"");
		if (!Ads.Instance.isSkipAdvertisement) {
				Ads.Instance.showAds ();
		}else{
			buttonSkipAdmod.SetActive(false);
		}
#endif

	}

	IEnumerator Lose()
	{
		gameState = GameState.Lose;

		EffectController.Instance.PlayCrushEffect();
		m_Audio.PlayOneShot(m_AudioCrush);

		GameValue.Ball.gameObject.SetActive(false);



		Resources.UnloadUnusedAssets();
		System.GC.Collect();

		yield return new WaitForSeconds(0.75f);

		m_Collider.enabled = false;

		GameValue.GameStar = 0;

		m_EndObj.SetActive(true);
		lbTest.text = "";
#if UNITY_ANDROID
		//AndroidMessage.Create("tat quang cao: ",AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased (GPaymnetManagerExample.ANDROID_TEST_PURCHASED)+"");
		if (!Ads.Instance.isSkipAdvertisement) {
			Ads.Instance.showAds();
		}else{
			buttonSkipAdmod.SetActive(false);
		}
#endif

		//DisplayRateDialog();
	}

	public void Begin()
	{
		gameState = GameState.Running;
	}

	public void Pause()
	{
		if (Time.timeScale == 1)
			Time.timeScale = 0;
		else if (Time.timeScale == 0)
			Time.timeScale = 1;
	}
	public void UnPause()
	{
		Time.timeScale = 1;
	}

	public void Bounce(Vector3 pos)
	{
		GameValue.BounceRemain--;

		if (GameValue.BounceRemain >= 0)
		{
			m_BounceLb.text = string.Empty + GameValue.BounceRemain;

			EffectController.Instance.PlayHitEffect(pos);

			m_Audio.PlayOneShot(m_AudioBounce[Random.Range(0, 2)]);
		}

//		if (GameValue.BounceRemain <= 0)
//			Lose()
	}

	public void IntoTheHole()
	{
		StartCoroutine(Lose());
	}

	public void hideAdBanner()
	{
		Ads.Instance.hideAdBanner();
	}	

	private void DisplayRateDialog()
	{
		if (PlayerPrefs.GetInt("rate") == 0 && !rated && Time.realtimeSinceStartup > 60*5)
		{
			DialogManager.Instance.SetLabel("Rate", "Cancel", "Close");
			DialogManager.Instance.ShowSelectDialog("Do you like this game? Pls rate and give us feedbacks.", (result) => {
				if (result)
					PlayerPrefs.SetInt("rate", 1);

				rated = true;
#if UNITY_ANDROID

		Application.OpenURL("https://play.google.com/store/apps/details?id=com.apohub.bouncingballshoot");

#elif UNITY_IOS
				float osVersion = -1f;
				string versionString = SystemInfo.operatingSystem.Replace("iPhone OS ", "");
				float.TryParse(versionString.Substring(0, 1), out osVersion);
				
				string appID;
#if NO_ADS
				appID = "954708039";
#else
				appID = "954185763";
#endif
				
				if (osVersion >= 7)
					Application.OpenURL("itms-apps://itunes.apple.com/app/id=" + appID);
				else
					Application.OpenURL("itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=" + appID);
#endif
			});
		}
	}
}
