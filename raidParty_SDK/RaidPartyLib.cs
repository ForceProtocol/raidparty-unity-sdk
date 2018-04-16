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
				Debug.Log ("RaidParty SDK running in test mode. Requests are sent to https://staging.hub.raidparty.io");
				this.raidparty_api_host = "https://staging.hub.raidparty.io/";
			}else{
				this.raidparty_api_host = "https://hub.raidparty.io/";
			}
		}

		private String makeApiRequest(String apiRoute, NameValueCollection requestParams) {
			try{
			
				System.Net.ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
			
				using (WebClient client = new WebClient())
				{
					var response = client.UploadValues(raidparty_api_host + apiRoute, requestParams);
					String responseString = Encoding.Default.GetString(response);

					Debug.Log ("RaidParty SDK web client success request. Response is: " + responseString);

					return responseString;
				}
			} catch (WebException ex) {

				var response = ex.Response as HttpWebResponse;

				if (ex.Status == WebExceptionStatus.ProtocolError) {
					if (response != null) {	
						return ((int)response.StatusCode).ToString ();
					} else {
						Debug.Log ("Protocol Error and response is null");
						return "404";
					}
				} else {
					Debug.Log ("Response is null");
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
				Debug.Log ("RaidParty SDK trackPlayer error. No player UID was provided.");
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
				Debug.Log ("RaidParty SDK trackEvent error. No player UID was provided.");
				return "raidPartyUid not found";
			}
			if (eventId.Length == 0) {
				Debug.Log ("RaidParty SDK trackEvent error. No event ID was provided.");
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

