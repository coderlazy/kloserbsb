public static class Constants 
{
	public readonly static float FACTOR_PIXEL_TO_METER = 0.015625f;
	public readonly static int SCREEN_HEIGHT_BY_PIXEL = 640;
	public readonly static int SCREEN_WIDTH_BY_PIXEL = 360;

	public readonly static string PATH_LEVELS = "Levels/level";
	public readonly static string PATH_EFFECTS = "Prefabs/Effects/";
	public readonly static string PATH_OBJECTS = "Prefabs/Objects/";
	public readonly static string PATH_WAYPOINTS = "Prefabs/Waypoints/";

	public readonly static int CHAPTER_DIVIDE_FACTOR = 12;

	public readonly static float BOUNCE_POSITION_FACTOR = -0.05f;
	////I pass level {0} Bouncing ball shooter  ‪#‎bouncingball‬ with only {1}times banging balls & xx seconds  , Can you do better?
	public readonly static string STRING_SHARING = "I got {0} star(s) with only {1:0.00}s to pass level {2} in #BouncingBall. Download and beat me {3}";
	public readonly static string STRING_LOSE_SHARING = "I can't beat level {0} in #BouncingBall. Anyone know how to beat this? {1}";

#if UNITY_ANDROID
	public readonly static string DOWNLOAD_URL = "https://play.google.com/store/apps/details?id=com.apohub.bouncingballshoot";
	public readonly static string ADMOB_BANNER_ID = "ca-app-pub-1996829160558431/5101553109";
	public readonly static string ADMOB_INTERSTITIAL_ID = "ca-app-pub-1996829160558431/9531752708";
	public readonly static string PURCHASE_SKIPP_ADS = "no_ads";
#elif UNITY_IOS
	public readonly static string DOWNLOAD_URL = "";
	public readonly static string ADMOB_BANNER_ID = "ca-app-pub-7014870114322102/4194584879";
	public readonly static string ADMOB_INTERSTITIAL_ID = "ca-app-pub-7014870114322102/2578250878";
#elif UNITY_WP8
	public readonly static string DOWNLOAD_URL = "";
	public readonly static string ADMOB_BANNER_ID = "ca-app-pub-7014870114322102/4194584879";
	public readonly static string ADMOB_INTERSTITIAL_ID = "ca-app-pub-7014870114322102/2578250878";
#endif
}
