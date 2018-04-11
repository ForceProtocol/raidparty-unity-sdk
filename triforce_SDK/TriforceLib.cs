using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Collections.Specialized;


namespace triforce_SDK
{
	public class TriforceLib
	{
		const String RAIDPARTY_API_HOST = "http://localhost:1337/";
		private String playerEmail, developerPublicKey, developerPrivateKey;

		public TriforceLib (String email, String public_key, String private_key)
		{
			this.playerEmail = email;
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
	
		public String trackPlayer() {
			String stringToEncrypt = "/sdk/player/track" + ":" + this.playerEmail + ":" + this.developerPublicKey + ":" 
				+ this.developerPrivateKey;
			String authKey = generateAuthKey(stringToEncrypt);
			var requestParams = new NameValueCollection();
			requestParams["public_key"] = this.developerPublicKey;
			requestParams["auth_key"] = authKey;
			requestParams["user_id"] = this.playerEmail;
			return this.makeApiRequest("sdk/player/track", requestParams);
		}
	
		public String rewardPlayerEvent(String eventDescription, String eventValue) {
			String eventName = "rewardEvent";
			String stringToEncrypt = "/sdk/game/event" + ":" + this.playerEmail + ":" + eventName + ":" + this.developerPublicKey + ":" 
				+ this.developerPrivateKey;
			String authKey = generateAuthKey(stringToEncrypt);
			var requestParams = new NameValueCollection();
			requestParams["public_key"] = this.developerPublicKey;
			requestParams["auth_key"] = authKey;
			requestParams["user_id"] = this.playerEmail;
			requestParams["event_name"] = eventName;
			requestParams ["event_description"] = eventDescription;
			requestParams ["event_value"] = eventValue;
			return this.makeApiRequest("sdk/game/event", requestParams);
		}
	}
}

