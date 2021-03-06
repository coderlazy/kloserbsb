////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using Facebook;
using System.Collections;
using System.Collections.Generic;

public class SPFacebook : SA_Singleton<SPFacebook> {


	private FacebookUserInfo _userInfo = null;
	private Dictionary<string,  FacebookUserInfo> _friends;
	private bool _IsInited = false;


	private  Dictionary<string,  FBScore> _userScores =  new Dictionary<string, FBScore>();
	private  Dictionary<string,  FBScore> _appScores  =  new Dictionary<string, FBScore>();

	private int lastSubmitedScore = 0;


	private  Dictionary<string,  Dictionary<string, FBLikeInfo>> _likes =  new Dictionary<string, Dictionary<string, FBLikeInfo>>();


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public void Init() {
		FB.Init(OnInitComplete, OnHideUnity);
	}


	public void Login() {
		Login(SocialPlatfromSettings.Instance.fb_scopes);
	}

	public void Login(string scopes) {
		Debug.Log("AndroidNative login");
		FB.Login(scopes, LoginCallback);

	}


	//--------------------------------------
	//  API METHODS
	//--------------------------------------


	public void Logout() {
		FB.Logout();
	}



	public void LoadUserData() {
		if(IsLoggedIn) {
			
			FB.API("/me", Facebook.HttpMethod.GET, UserDataCallBack);  
			
		} else {
			Debug.LogWarning("Auth user before loadin data, fail event generated");
			dispatch(FacebookEvents.USER_DATA_FAILED_TO_LOAD, null);
		}
	}

	public void LoadFrientdsInfo(int limit) {
		if(IsLoggedIn) {
			
			FB.API("/me?fields=friends.limit(" + limit.ToString() + ").fields(first_name,id,last_name,name,link,locale,location)", Facebook.HttpMethod.GET, FriendsDataCallBack);  

		} else {
			Debug.LogWarning("Auth user before loadin data, fail event generated");
			dispatch(FacebookEvents.FRIENDS_FAILED_TO_LOAD, null);
		}
	}

	public FacebookUserInfo GetFrindById(string id) {
		if(_friends != null) {
			if(_friends.ContainsKey(id)) {
				return _friends[id];
			}
		}

		return null;
	}




	public void PostImage(string caption, Texture2D image) {


		byte[] imageBytes = image.EncodeToPNG();

		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("message", caption);
		wwwForm.AddBinaryData("image", imageBytes, "InteractiveConsole.png");


		FB.API("me/photos", Facebook.HttpMethod.POST, PostCallBack, wwwForm);


	}


	public FBPostingTask PostWithAuthCheck(
		string toId = "",
		string link = "",
		string linkName = "",
		string linkCaption = "",
		string linkDescription = "",
		string picture = "",
		string actionName = "",
		string actionLink = "",
		string reference = ""
		) {

		FBPostingTask task = FBPostingTask.Cretae();

		task.Post(toId,
		          link,
		          linkName,
		          linkCaption,
		          linkDescription,
		          picture,
		          actionName,
		          actionLink,
		          reference);


		return task;

	} 



	public void Post (
								string toId = "",
								string link = "",
								string linkName = "",
								string linkCaption = "",
								string linkDescription = "",
								string picture = "",
								string actionName = "",
								string actionLink = "",
								string reference = ""
							) 
	{

		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before posting, fail event generated");
			dispatch(FacebookEvents.POST_FAILED, null);
			return;
		}

		FB.Feed(
			toId: toId,
			link: link,
			linkName: linkName,
			linkCaption: linkCaption,
			linkDescription: linkDescription,
			picture: picture,
			actionName : actionName,
			actionLink : actionLink,
			reference : reference,
			callback: PostCallBack
			);

	}





	public void AppRequest(

		string message,
		OGActionType actionType,
		string objectId,
		string[] to,
		string data = "",
		string title = "")
	{

		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before AppRequest, fail event generated");
			dispatch(FacebookEvents.APP_REQUEST_COMPLETE, new FBResult("","User isn't authed"));
			return;
		}

		FB.AppRequest(message, actionType, objectId, to, data, title, AppRequestCallBack);
	}
	
	public void AppRequest(
		string message,
		OGActionType actionType,
		string objectId,
		List<object> filters = null,
		string[] excludeIds = null,
		int? maxRecipients = null,
		string data = "",
		string title = "")
	{
		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before AppRequest, fail event generated");
			dispatch(FacebookEvents.APP_REQUEST_COMPLETE, new FBResult("","User isn't authed"));
			return;
		}



		FB.AppRequest(message, actionType, objectId, filters, excludeIds, maxRecipients, data, title, AppRequestCallBack);
	}
	
	public void AppRequest(
		string message,
		string[] to = null,
		List<object> filters = null,
		string[] excludeIds = null,
		int? maxRecipients = null,
		string data = "",
		string title = "")
	{
		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before AppRequest, fail event generated");
			dispatch(FacebookEvents.APP_REQUEST_COMPLETE, new FBResult("","User isn't authed"));
			return;
		}

		FB.AppRequest(message, to, filters, excludeIds, maxRecipients, data, title, AppRequestCallBack);
	}



	//--------------------------------------
	//  Scores API 
	//  https://developers.facebook.com/docs/games/scores
	//------------------------------------
	
	
	
	//Read score for a player
	public void LoadPlayerScores() {
		FB.API("/" + FB.UserId + "/scores", Facebook.HttpMethod.GET, OnLoaPlayrScoresComplete);  
	}
	
	//Read scores for players and friends
	public void LoadAppScores() {
		FB.API("/" + FB.AppId + "/scores", Facebook.HttpMethod.GET, OnAppScoresComplete);  
	}
	
	//Create or update a score
	public void SubmitScore(int score) {
		lastSubmitedScore = score;
		FB.API("/" + FB.UserId + "/scores?score=" + score, Facebook.HttpMethod.POST, OnScoreSubmited);  
	}
	
	//Delete scores for a player
	public void DeletePlayerScores() {
		FB.API("/" + FB.UserId + "/scores", Facebook.HttpMethod.DELETE, OnScoreDeleted); 


	}



	//--------------------------------------
	//  Likes API 
	//  https://developers.facebook.com/docs/graph-api/reference/v2.0/user/likes
	//------------------------------------

	public void LoadCurrentUserLikes() {
		LoadLikes(FB.UserId);
	}


	public void LoadLikes(string userId) {
		FBLikesRetriveTask task = FBLikesRetriveTask.Create();
		task.addEventListener(BaseEvent.COMPLETE, OnUserLikesResult);
		task.LoadLikes(userId);
	}

	public void LoadLikes(string userId, string pageId) {
		FBLikesRetriveTask task = FBLikesRetriveTask.Create();
		task.addEventListener(BaseEvent.COMPLETE, OnUserLikesResult);
		task.LoadLikes(userId, pageId);
	}




	//--------------------------------------
	//  Payment API 
	//  https://developers.facebook.com/docs/unity/reference/current/FB.Canvas.Pay
	//------------------------------------


	public void Pay (string product,  int quantity = 1) {
		Pay (product, "purchaseitem", quantity);
	}

	public void Pay ( string product,
	                 string action = "purchaseitem",
	                 int quantity = 1,
	                 int? quantityMin = null,
	                 int? quantityMax = null,
	                 string requestId = null,
	                 string pricepointId = null,
	                 string testCurrency = null
	                 ) {



		FB.Canvas.Pay (product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, FBPaymentCallBack);
	}


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------



	public FBScore GetCurrentPlayerScoreByAppId(string appId) {
		if(_userScores.ContainsKey(appId)) {
			return _userScores[appId];
		} else {
			FBScore score =  new FBScore();
			score.UserId = FB.UserId;
			score.AppId = appId;
			score.value = 0;

			return score;
		}
	}
	

	public int GetCurrentPlayerIntScoreByAppId(string appId) {
		return GetCurrentPlayerScoreByAppId(appId).value;
	}




	public int GetScoreByUserId(string userId) {
		if(_appScores.ContainsKey(userId)) {
			return _appScores[userId].value;
		} else {
			return 0;
		}
	}



	public List<FBLikeInfo> GerUserLikesList(string userId){
	
		List<FBLikeInfo>  result = new List<FBLikeInfo>();

		if(_likes.ContainsKey(userId)) {
			foreach(KeyValuePair<string,  FBLikeInfo>  pair in _likes[userId]) {
				result.Add(pair.Value);
			}
		}

		return result;
	}

	public bool IsUserLikesPage(string userId, string pageId) {
		if(_likes.ContainsKey(userId)) {
			if(_likes[userId].ContainsKey(pageId)) {
				return true;
			}
		}

		return false;
	}


	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------


	public bool IsInited  {
		get {
			return _IsInited;
		}
	}

	public bool IsLoggedIn {
		get {
			return FB.IsLoggedIn;
		}
	}


	public string UserId {
		get {
			return FB.UserId;
		}
	}

	public string AccessToken {
		get {
			return FB.AccessToken;
		}
	}

	public FacebookUserInfo userInfo {
		get {
			return _userInfo;
		}
	}

	public Dictionary<string,  FacebookUserInfo> friends {
		get {
			return _friends;
		}
	}

	public List<string> friendsIds {
		get {
			if(_friends == null) {
				return null;
			}

			List<string> ids = new List<string>();
			foreach(KeyValuePair<string, FacebookUserInfo> item in _friends) {
				ids.Add(item.Key);
			}

			return ids;
		}
	}

	public List<FacebookUserInfo> friendsList {
		get {
			if(_friends == null) {
				return null;
			}
			
			List<FacebookUserInfo> flist = new List<FacebookUserInfo>();
			foreach(KeyValuePair<string, FacebookUserInfo> item in _friends) {
				flist.Add(item.Value);
			}

			return flist;
		}
	}


	 
	public  Dictionary<string,  FBScore> userScores  {
		get {
			return _userScores;
		}
	}

	public  Dictionary<string,  FBScore>  appScores{
		get {
			return _appScores;
		}
	}


	public List<FBScore> applicationScoreList {
		get {
			List<FBScore>  result = new List<FBScore>();
			foreach(KeyValuePair<string,  FBScore>  pair in _appScores) {
				result.Add(pair.Value);
			}

			return result;
		}
	}








	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnUserLikesResult(CEvent e) {


		FBLikesRetriveTask task = e.dispatcher as FBLikesRetriveTask;
		task.removeEventListener(BaseEvent.COMPLETE, OnUserLikesResult);

		FBResult result = e.data as FBResult;


		FB_APIResult r;
		if(result.Error != null) {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.LIKES_LIST_LOADED, r);
			return;
		}


		Dictionary<string, object> JSON = ANMiniJSON.Json.Deserialize(result.Text) as Dictionary<string, object>;
		List<object> data = JSON["data"]  as List<object>;


		Dictionary<string, FBLikeInfo> userLikes = null;
		if(_likes.ContainsKey(task.userId)) {
			userLikes = _likes[task.userId];
		} else {
			userLikes =  new Dictionary<string, FBLikeInfo>();
			_likes.Add(task.userId, userLikes);
		}

		foreach(object row in data) {
			Dictionary<string, object> dataRow = row as Dictionary<string, object>;

			FBLikeInfo tpl =  new FBLikeInfo();
			tpl.id 			= System.Convert.ToString(dataRow["id"]);
			tpl.name 		= System.Convert.ToString(dataRow["name"]);
			tpl.category 	= System.Convert.ToString(dataRow["category"]);

			if(userLikes.ContainsKey(tpl.id)) {
				userLikes[tpl.id] = tpl;
			} else {
				userLikes.Add(tpl.id, tpl);
			}
		}

		r = new FB_APIResult(true, result.Text);
		r.Unity_FB_Result = result;
		dispatch(FacebookEvents.LIKES_LIST_LOADED, r);
	}




	private void OnScoreDeleted(FBResult result) {
		FB_APIResult r;
		if(result.Error != null) {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.DELETE_SCORES_REQUEST_COMPLETE, r);
			return;
		}


		if(result.Text.Equals("true")) {
			r = new FB_APIResult(true, result.Text);
			r.Unity_FB_Result = result;
			
			FBScore score = new FBScore();
			score.AppId = FB.AppId;
			score.UserId = FB.UserId;
			score.value = 0;
			
			if(_appScores.ContainsKey(FB.UserId)) {
				_appScores[FB.UserId].value = 0;
			}  else {
				_appScores.Add(score.UserId, score);
			}
			
			
			if(_userScores.ContainsKey(FB.AppId)) {
				_userScores[FB.AppId].value = 0;
			} else {
				_userScores.Add(FB.AppId, score); 
			}
			
			
			dispatch(FacebookEvents.DELETE_SCORES_REQUEST_COMPLETE, r);
			
			
		} else {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.DELETE_SCORES_REQUEST_COMPLETE, r);
		}


	}

	private void OnScoreSubmited(FBResult result) {

		FB_APIResult r;
		if(result.Error != null) {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.SUBMIT_SCORE_REQUEST_COMPLETE, r);
			return;
		}


		if(result.Text.Equals("true")) {
			r = new FB_APIResult(true, result.Text);
			r.Unity_FB_Result = result;

			FBScore score = new FBScore();
			score.AppId = FB.AppId;
			score.UserId = FB.UserId;
			score.value = lastSubmitedScore;

			if(_appScores.ContainsKey(FB.UserId)) {
				_appScores[FB.UserId].value = lastSubmitedScore;
			}  else {
				_appScores.Add(score.UserId, score);
			}


			if(_userScores.ContainsKey(FB.AppId)) {
				_userScores[FB.AppId].value = lastSubmitedScore;
			} else {
				_userScores.Add(FB.AppId, score); 
			}


			dispatch(FacebookEvents.SUBMIT_SCORE_REQUEST_COMPLETE, r);


		} else {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.SUBMIT_SCORE_REQUEST_COMPLETE, r);
		}
	}


	private void OnAppScoresComplete(FBResult result) {
		FB_APIResult r;
		if(result.Error != null) {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.APP_SCORES_REQUEST_COMPLETE, r);
			return;
		}
		
		Dictionary<string, object> JSON = ANMiniJSON.Json.Deserialize(result.Text) as Dictionary<string, object>;
		List<object> data = JSON["data"]  as List<object>;
		
		foreach(object row in data) {
			FBScore score =  new FBScore();
			Dictionary<string, object> dataRow = row as Dictionary<string, object>;
			
			Dictionary<string, object> userInfo = dataRow["user"]  as Dictionary<string, object>;
			
			score.UserId = System.Convert.ToString(userInfo["id"]);
			score.UserName = System.Convert.ToString(userInfo["name"]);
			
			
			score.value = System.Convert.ToInt32(dataRow["score"]); 

			Dictionary<string, object> AppInfo = dataRow["application"]  as Dictionary<string, object>;
			
			score.AppId = System.Convert.ToString(AppInfo["id"]);
			score.AppName = System.Convert.ToString(AppInfo["name"]);
			
			AddToAppScores(score);


		}
		
		r = new FB_APIResult(true, result.Text);
		r.Unity_FB_Result = result;
		dispatch(FacebookEvents.APP_SCORES_REQUEST_COMPLETE, r);
	}

	private void AddToAppScores(FBScore score) {

		if(_appScores.ContainsKey(score.UserId)) {
			_appScores[score.UserId] = score;
		} else {
			_appScores.Add(score.UserId, score);
		}

		if(_userScores.ContainsKey(score.AppId)) {
			_userScores[score.AppId] = score;
		} else {
			_userScores.Add(score.AppId, score);
		}




	}

	private void AddToUserScores(FBScore score) {
		if(_userScores.ContainsKey(score.AppId)) {
			_userScores[score.AppId] = score;
		} else {
			_userScores.Add(score.AppId, score);
		}


		if(_appScores.ContainsKey(score.UserId)) {
			_appScores[score.UserId] = score;
		} else {
			_appScores.Add(score.UserId, score);
		}

	}

	private void OnLoaPlayrScoresComplete(FBResult result) {
	

		FB_APIResult r;
		if(result.Error != null) {
			r = new FB_APIResult(false, result.Error);
			r.Unity_FB_Result = result;
			dispatch(FacebookEvents.PLAYER_SCORES_REQUEST_COMPLETE, r);
			return;
		}

		Dictionary<string, object> JSON = ANMiniJSON.Json.Deserialize(result.Text) as Dictionary<string, object>;
		List<object> data = JSON["data"]  as List<object>;

		foreach(object row in data) {
			FBScore score =  new FBScore();
			Dictionary<string, object> dataRow = row as Dictionary<string, object>;

			Dictionary<string, object> userInfo = dataRow["user"]  as Dictionary<string, object>;

			score.UserId = System.Convert.ToString(userInfo["id"]);
			score.UserName = System.Convert.ToString(userInfo["name"]);


			score.value = System.Convert.ToInt32(dataRow["score"]); 


			Dictionary<string, object> AppInfo = dataRow["application"]  as Dictionary<string, object>;

			score.AppId = System.Convert.ToString(AppInfo["id"]);
			score.AppName = System.Convert.ToString(AppInfo["name"]);


			AddToUserScores(score);

		}

		r = new FB_APIResult(true, result.Text);
		r.Unity_FB_Result = result;
		dispatch(FacebookEvents.PLAYER_SCORES_REQUEST_COMPLETE, r);

	}

	private void resultTest(FBResult result) {
		Debug.Log(result.Error);
		Debug.Log(result.Text);
	}


	private void OnScoreRequestComplete(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.COMPLETE, OnScoreRequestComplete);

	 	FB_APIResult result = e.data as FB_APIResult;
		Debug.Log(result.responce);
	}


	private void PostCallBack(FBResult result) {
		if (result.Error != null)  {                                                                                                                          
			Debug.LogWarning(result.Error);
			dispatch(FacebookEvents.POST_FAILED, result);
			return;
		}          

		dispatch(FacebookEvents.POST_SUCCEEDED, result);
	}


	private void AppRequestCallBack(FBResult result) {
		dispatch(FacebookEvents.APP_REQUEST_COMPLETE, result);
	}


	private void FriendsDataCallBack(FBResult result) {
		if (result.Error != null)  {                                                                                                                          
			Debug.LogWarning(result.Error);
			dispatch(FacebookEvents.FRIENDS_FAILED_TO_LOAD, result);
			
			return;
		}          
		
		ParceFriendsData(result.Text);
		dispatch(FacebookEvents.FRIENDS_DATA_LOADED, result);
	}


	public void ParceFriendsData(string data) {

		Debug.Log("ParceFriendsData");
		Debug.Log(data);

		try {
			_friends =  new Dictionary<string, FacebookUserInfo>();
			IDictionary JSON =  ANMiniJSON.Json.Deserialize(data) as IDictionary;	
			IDictionary f = JSON["friends"] as IDictionary;
			IList flist = f["data"] as IList;


			for(int i = 0; i < flist.Count; i++) {
				FacebookUserInfo user = new FacebookUserInfo(flist[i] as IDictionary);
				_friends.Add(user.id, user);
			}

		} catch(System.Exception ex) {
			Debug.LogWarning("Parceing Friends Data failed");
			Debug.LogWarning(ex.Message);
		}

	}

	private void ScoreLoadResult(FBResult result) {
		Debug.Log(result.Text);
	}

	private void UserDataCallBack(FBResult result) {
		if (result.Error != null)  {         

			Debug.LogWarning(result.Error);
			dispatch(FacebookEvents.USER_DATA_FAILED_TO_LOAD, result);

			return;
		}          

		_userInfo = new FacebookUserInfo(result.Text);
		dispatch(FacebookEvents.USER_DATA_LOADED, result);

	}


	private void OnInitComplete() {
		_IsInited = true;
		dispatch(FacebookEvents.FACEBOOK_INITED);

		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}



	private void OnHideUnity(bool isGameShown) {
		dispatch(FacebookEvents.GAME_FOCUS_CHANGED, isGameShown);
	}


	
	private void LoginCallback(FBResult result) {
		if(FB.IsLoggedIn) {
			dispatch(FacebookEvents.AUTHENTICATION_SUCCEEDED, result);
		} else {
			dispatch(FacebookEvents.AUTHENTICATION_FAILED, result);
		}
	}

	private void FBPaymentCallBack (FBResult result) {
		if(FB.IsLoggedIn) {
			dispatch(FacebookEvents.PAYMENT_SUCCEEDED, result);
		} else {
			dispatch(FacebookEvents.PAYMENT_FAILED, result);
		}
	}	

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
