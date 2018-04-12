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
		const String RAIDPARTY_API_HOST = "http://localhost:1337/";
		private String developerPublicKey, developerPrivateKey;

		public RaidPartyLib (String public_key, String private_key)
		{
			this.developerPublicKey = public_key;
			this.developerPrivateKey = private_key;
		}

		private String makeApiRequest(String apiRoute, NameValueCollection requestParams) {
			using (WebClient client = new WebClient())
			{
				var response = client.UploadValues(RAIDPARTY_API_HOST + apiRoute, requestParams);
				String responseString = Encoding.Default.GetString(response);
				return responseString;
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
	
		public String trackPlayer(String playerId) {
			if (playerId == "") {
				return "Invalid playerId";
			}
			PlayerPrefs.SetString ("playerId", playerId);
			String stringToEncrypt = "/sdk/player/track" + ":" + playerId + ":" + this.developerPublicKey + ":" 
				+ this.developerPrivateKey;
			String authKey = generateAuthKey(stringToEncrypt);
			var requestParams = new NameValueCollection();
			requestParams["public_key"] = this.developerPublicKey;
			requestParams["auth_key"] = authKey;
			requestParams["user_id"] = playerId;
			String res = this.makeApiRequest("sdk/player/track", requestParams);
		}
	
		public String trackEvent(String eventName, String eventDescription, String eventValue) {
			String playerId = PlayerPrefs.GetString ("playerId");
			if (playerId == "") {
				return "playerId not found";
			}
			if (eventName.Length == 0 || eventDescription.Length == 0 || eventDescription.Length > 140) {
				return "Missing/Invalid eventName and/or eventDescription";
			}
			String stringToEncrypt = "/sdk/game/event" + ":" + playerId + ":" + eventName + ":" + this.developerPublicKey + ":" 
				+ this.developerPrivateKey;
			String authKey = generateAuthKey(stringToEncrypt);
			var requestParams = new NameValueCollection();
			requestParams["public_key"] = this.developerPublicKey;
			requestParams["auth_key"] = authKey;
			requestParams["user_id"] = playerId;
			requestParams["event_name"] = eventName;
			requestParams ["event_description"] = eventDescription;
			requestParams ["event_value"] = eventValue;
			return this.makeApiRequest("sdk/game/event", requestParams);
		}
	}
}

