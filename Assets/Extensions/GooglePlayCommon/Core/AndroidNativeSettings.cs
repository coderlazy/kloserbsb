﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class AndroidNativeSettings : ScriptableObject {

	public const string VERSION_NUMBER = "4.4";


	public bool EnableGamesAPI 		= true;
	public bool EnableAppStateAPI 	= true;
	public bool LoadProfileIcons 	= true;
	public bool LoadProfileImages 	= true;

	public bool LoadQuestsImages 	= true;
	public bool LoadQuestsIcons 	= true;
	public bool LoadEventsIcons 	= true;


	public bool ShowStoreKitParams = false;
	public bool ShowPSSettings = false;
	public bool ShowActions = false;


	public string base64EncodedPublicKey = "REPLACE_WITH_YOUR_PUBLIC_KEY";
	public List<string> InAppProducts = new List<string>();





	public const string ANSettingsAssetName = "AndroidNativeSettings";
	public const string ANSettingsPath = "Extensions/AndroidNative/Resources";
	public const string ANSettingsAssetExtension = ".asset";

	private static AndroidNativeSettings instance = null;

	
	public static AndroidNativeSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(ANSettingsAssetName) as AndroidNativeSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<AndroidNativeSettings>();
					#if UNITY_EDITOR
					//string properPath = Path.Combine(Application.dataPath, ANSettingsPath);

					FileStaticAPI.CreateFolder(ANSettingsPath);

					/*
					if (!Directory.Exists(properPath)) {
						AssetDatabase.CreateFolder("Extensions/", "AndroidNative");
						AssetDatabase.CreateFolder("Extensions/AndroidNative", "Resources");
					}
					*/
					
					string fullPath = Path.Combine(Path.Combine("Assets", ANSettingsPath),
					                               ANSettingsAssetName + ANSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}


	public bool IsBase64KeyWasReplaced {
		get {
			if(base64EncodedPublicKey.Equals("REPLACE_WITH_YOUR_PUBLIC_KEY")) {
				return false;
			} else {
				return true;
			}
		}
	}

}
