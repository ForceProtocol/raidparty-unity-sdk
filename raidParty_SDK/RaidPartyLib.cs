using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Networking;

namespace raidParty_SDK
{
	public class RaidPartyLib
	{
		private String app_id, app_key, raidparty_api_host;

		public RaidPartyLib (String app_id, String app_key, bool testing)
		{
			this.app_id = app_id;
			this.app_key = app_key;
			
			if(testing){
				this.raidparty_api_host = "https://staging.hub.raidparty.io/";
			}else{
				this.raidparty_api_host = "https://hub.raidparty.io/";
			}
		}

		private WWW makeApiRequest(String apiRoute, WWWForm data) {
			WWW www = new WWW(this.raidparty_api_host + apiRoute, data);
			WaitForSeconds w;
			while (!www.isDone)
				w = new WaitForSeconds (0.1f);
			return www;
		}

		private String generateAuthKey(String stringToEncrypt) {
			String sh1Hash = String.Empty;
			SHA1 crypt = new SHA1CryptoServiceProvider ();
			byte[] hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(stringToEncrypt));
			foreach (byte hashByte in hash) {
				sh1Hash += hashByte.ToString ("x2");
			}
			return sh1Hash;
		}

		private int getResponseCode(WWW response) {
			int ret = 0;
			if (response.responseHeaders == null) {
				Debug.LogError("no response headers.");
				return 404;
			}
			else {
				if (!response.responseHeaders.ContainsKey("STATUS")) {
					Debug.LogError("response headers has no STATUS.");
					return 404; 
				}
				else {
					String statusLine = response.responseHeaders ["STATUS"];
					string[] components = statusLine.Split(' ');
					if (components.Length < 3) {
						Debug.LogError("invalid response status: " + statusLine);
						return 404;
					}
					else {
						if (!int.TryParse(components[1], out ret)) {
							Debug.LogError("invalid response code: " + components[1]);
							return 404;
						}
					}
				}
			}
			return ret;
		}

		/**
		* Method to track player login activity through SDK.
		*/
		public int trackPlayer(String raidPartyUid) {
			String stringToEncrypt = "/sdk/player/track" + ":" + raidPartyUid + ":" + this.app_id + ":" 
				+ this.app_key;
			String authKey = generateAuthKey(stringToEncrypt);
			WWWForm form = new WWWForm ();
			form.AddField("public_key", this.app_id);
			form.AddField("auth_key", authKey);
			form.AddField("user_id", raidPartyUid);
			WWW response = this.makeApiRequest("sdk/player/track", form);
			if (response.error == null) {
				PlayerPrefs.SetString ("raidPartyUid", raidPartyUid);
				return 201;
			} else {
				return getResponseCode (response);
			}
		}
	
		/**
		* Method to generate events through SDK.
		*/
		public int trackEvent(String eventId, String eventValue) {
			String raidPartyUid = PlayerPrefs.GetString ("raidPartyUid");
			if (raidPartyUid == "") {
				Debug.LogError("raidPartyUid not found");
				return 405;
			}
			if (eventId.Length == 0) {
				Debug.LogError("Missing/Invalid eventId");
				return 406;
			}
			String stringToEncrypt = "/sdk/game/event" + ":" + raidPartyUid + ":" + eventId + ":" + this.app_id + ":" 
				+ this.app_key;
			String authKey = generateAuthKey(stringToEncrypt);
			WWWForm form = new WWWForm ();
			form.AddField("public_key", this.app_id);
			form.AddField("auth_key", authKey);
			form.AddField("user_id", raidPartyUid);
			form.AddField("event_id", eventId);
			form.AddField("event_value", eventValue);
			WWW response = this.makeApiRequest("sdk/game/event", form);
			if (response.error == null) {
				PlayerPrefs.SetString ("raidPartyUid", raidPartyUid);
				return 201;
			} else {
				return getResponseCode (response);
			}
		}
		
		/**
		* Method to get the stored player ID for use in the game code
		*/
		public String getPlayerId(){
			String raidPartyUid = PlayerPrefs.GetString ("raidPartyUid");
			if (raidPartyUid == "") {
				return "";
			}
			return raidPartyUid;
		}



		/**
		* Make socket connection to request dynamic adverts
		*/
		public String findAdvertForAsset(String gameAssetId){
			
		}
	}
}

