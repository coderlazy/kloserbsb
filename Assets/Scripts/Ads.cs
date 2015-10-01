using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class Ads : MonoSingleton<Ads> {
	
	private BannerView m_BannerViewBot;
	private BannerView m_BannerViewTop;
	private InterstitialAd m_InterstitialAd;
	private AdRequest request;
	private string MY_BANNERS_AD_UNIT_ID;//		 = "ca-app-pub-9386605365016531/7732684605"; 
	private string MY_INTERSTISIALS_AD_UNIT_ID;//= "ca-app-pub-9386605365016531/9209417805"; 
	private GoogleMobileAdBanner banner1;
	private GoogleMobileAdBanner banner2;
	public int showAdsCount;
	public UILabel test;
	private int showInterstitialMilestone;
	private bool isShowInterstitial;
	public bool isSkipAdvertisement;
	int start = 0;
	public void init() {
		GPaymnetManagerExample.init ();
	}
	public void EnableAdvertisement(){
		MY_BANNERS_AD_UNIT_ID = Constants.ADMOB_BANNER_ID;
		MY_INTERSTISIALS_AD_UNIT_ID = Constants.ADMOB_INTERSTITIAL_ID;
		AndroidAdMobController.instance.Init(MY_BANNERS_AD_UNIT_ID);
		//AndroidMessage.Create("tat quang cao: ",AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased (GPaymnetManagerExample.ANDROID_TEST_PURCHASED)+"");
		AndroidAdMobController.instance.SetInterstisialsUnitID(MY_INTERSTISIALS_AD_UNIT_ID);
		//SmartBottom ();
		SmartTop ();
		isSkipAdvertisement = false;
	}
	// Use this for initialization
	void Start () {
		isSkipAdvertisement = false;
		init ();
		showAdsCount = 0;
	}

	void Update(){
		if(Application.loadedLevelName.Equals("Game")){
			if(test == null){
				//test = GameObject.Find("LabelTest").GetComponent<UILabel>();
			}else{
				//test.text = showAdsCount + " " + isSkipAdvertisement;
			}
		}
		if(GPaymnetManagerExample.isInited && !Ads.Instance.isSkipAdvertisement){
			if(AndroidInAppPurchaseManager.instance.inventory.purchases.Count != 0){
				Ads.Instance.isSkipAdvertisement = true;
			}
		}
//		if(!GPaymnetManagerExample.isInited){
//			GPaymnetManagerExample.init ();
//		}
	}
	public void showAds()
	{
		//B2Show ();
		B1Show ();
		showAdsCount++;
		if (showAdsCount >= 5)
		{
			showAdsCount = 0;
			StartInterstitialAd ();
		}
	}
	
	public void hideAdBanner()
	{
		B2Hide ();
		B1Hide ();
		//RefreshAds();
	}

	#region me
	//popup
	private void StartInterstitialAd() {
		AndroidAdMobController.instance.StartInterstitialAd ();
	}
	
	private void LoadInterstitialAd() {
		AndroidAdMobController.instance.LoadInterstitialAd ();
	}
	
	private void ShowInterstitialAd() {
		AndroidAdMobController.instance.ShowInterstitialAd ();
	}
	//banner top
	private void SmartTop() {
		banner1 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.UpperCenter, GADBannerSize.SMART_BANNER);
	}
	
	private void B1Hide() {
		banner1.Hide();
	}
	
	private void B1Show() {
		banner1.Show();
	}
	
	private void B1Refresh() {
		banner1.Refresh();
	}
	//banner bot
	private void SmartBottom() {
		//banner2 = AndroidAdMobController.instance.CreateAdBanner(TextAnchor.LowerCenter, GADBannerSize.SMART_BANNER);
	}

	private void B2Hide() {
		//banner2.Hide();
	}
	
	private void B2Show() {
		//banner2.Show();
	}
	
	private void B2Refresh() {
		//banner2.Refresh();
	}
	public void B2Destroy(){
		//AndroidAdMobController.instance.DestroyBanner(banner2.id);
	}
	#endregion


	private void RefreshAds()
	{
		B2Refresh();
		B1Refresh();
		LoadInterstitialAd();
	}
	IEnumerator ScheduleShowBot(){
#if UNITY_ANDROID
		yield return new WaitForSeconds(5);
		//B2Show();
		yield return new WaitForSeconds(3);
		//B2Hide();
		StartCoroutine (ScheduleShowBot());
#endif

	}
	#region purchase
	//buy item in app
	public void SuccsesPurchase() {
		if(GPaymnetManagerExample.isInited) {
			AndroidInAppPurchaseManager.instance.purchase (Constants.PURCHASE_SKIPP_ADS);
		} else {
			//AndroidMessage.Create("Error", "PaymnetManagerExample not yet inited");
		}
	}
	
	
	public void FailPurchase() {
		if(GPaymnetManagerExample.isInited) {
			AndroidInAppPurchaseManager.instance.purchase (GPaymnetManagerExample.ANDROID_TEST_ITEM_UNAVALIABLE);
		} else {
			//AndroidMessage.Create("Error", "PaymnetManagerExample not yet inited");
		}
	}
	
	
	public void ConsumeProduct() {
		if(GPaymnetManagerExample.isInited) {
			if(AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(GPaymnetManagerExample.ANDROID_TEST_PURCHASED)) {
				GPaymnetManagerExample.consume (GPaymnetManagerExample.ANDROID_TEST_PURCHASED);
			} else {
				//AndroidMessage.Create("Error", "You do not own product to consume it");
			}
			
		} else {
			//AndroidMessage.Create("Error", "PaymnetManagerExample not yet inited");
		}
	}
	#endregion purchase
}
