using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using UnityEngine;

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
				this.raidparty_api_host = "https://staging.hub.raidparty.io";
			}else{
				this.raidparty_api_host = "https://hub.raidparty.io";
			}
		}

		private String makeApiRequest(String apiRoute, NameValueCollection requestParams) {
			try{
				using (WebClient client = new WebClient())
				{
					var response = client.UploadValues(raidparty_api_host + apiRoute, requestParams);
					String responseString = Encoding.Default.GetString(response);
					return responseString;
				}
			} catch (WebException ex) {
				if (ex.Status == WebExceptionStatus.ProtocolError) {
					var response = ex.Response as HttpWebResponse;
					if (response != null) {	
						return ((int)response.StatusCode).ToString ();
					} else {
						return "404";
					}
				} else {
					return "404";
				}
			}
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
	
		public String trackPlayer(String raidPartyUid) {
			if (raidPartyUid == "") {
				return "Invalid raidPartyUid";
			}
			String stringToEncrypt = "/sdk/player/track" + ":" + raidPartyUid + ":" + this.app_id + ":" 
				+ this.app_key;
			String authKey = generateAuthKey(stringToEncrypt);
			var requestParams = new NameValueCollection();
			requestParams["public_key"] = this.app_id;
			requestParams["auth_key"] = authKey;
			requestParams["user_id"] = raidPartyUid;
			String responseCode = this.makeApiRequest("sdk/player/track", requestParams);
			if (responseCode == "201") 
			{
				PlayerPrefs.SetString ("raidPartyUid", raidPartyUid);
			}
			return responseCode;
		}
	
		public String trackEvent(String eventId, String eventValue) {
			String raidPartyUid = PlayerPrefs.GetString ("raidPartyUid");
			if (raidPartyUid == "") {
				return "raidPartyUid not found";
			}
			if (eventId.Length == 0) {
				return "Missing/Invalid eventId";
			}
			String stringToEncrypt = "/sdk/game/event" + ":" + raidPartyUid + ":" + eventId + ":" + this.app_id + ":" 
				+ this.app_key;
			String authKey = generateAuthKey(stringToEncrypt);
			var requestParams = new NameValueCollection();
			requestParams["public_key"] = this.app_id;
			requestParams["auth_key"] = authKey;
			requestParams["user_id"] = raidPartyUid;
			requestParams["event_id"] = eventId;
			requestParams ["event_value"] = eventValue;
			return this.makeApiRequest("sdk/game/event", requestParams);
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
	}
}

