using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using raidParty_SDK; // Include the raidParty_SDK.dll

public class raidPartyUnityExample : MonoBehaviour {

	public InputField playerUid; // Unique 7 character code the player enters in game settings

	// Instantiate the RaidParty SDK
	// new RaidPartyLib ("<Your Game App ID from RaidParty>", "<Your Game App KEY from RaidParty>", <bool Is this for testing? true = test environment>);
	private RaidPartyLib raidPartySdk = new RaidPartyLib ("57a776af-2dfd-4f1f-85bd-c065c8d14f34", "ea81e017-c07e-4bd4-91c5-4b47ef59d6c3", true);


	void Start () {
		if (raidPartySdk.getPlayerId () == "") {
			// The player is NOT being tracked if here. So we need the player to submit
			// their unique 7 character code in a form/input field
			// Take the value they enter, and send through raidPartySdk.trackPlayer("<players 7 character code>");
			Debug.Log ("The player is not being tracked by the SDK");
		} else {
			// The player is already being tracked, so they will not need to complete
			// this step again. You can show the player a connected already message
			// e.g. "You are already connected to the RaidParty reward platform"
			Debug.Log ("The player is already being tracked with a player UID of: " + raidPartySdk.getPlayerId ());
		}
	}


	/**
	 * The player clicks connect button
	 * Send the unique 7 character player ID the player entered
	 * and send through the RaidParty SDK
	 * */
	public void connectRequest() {
		
		Debug.Log ("the player ID entered is: " + playerUid.text);

		// Attempt to track the player with the code they provide
		int connectResponse = raidPartySdk.trackPlayer (playerUid.text);

		Debug.Log ("raidpartysdk response is: " + connectResponse);

		// If we get a success 201 response, the player is now connected.
		// You should display a success message to the player
		// e.g. "You are now connected to the RaidParty reward platform!"
		if (connectResponse == 201) {
			Debug.Log ("Player has now been connected");
		} 
		// There was an error, check the debug logs
		// Most likely cause, the player entered an invalid code
		// Ask the player to check their code and re-enter it
		else if (connectResponse == 404) {
			Debug.Log ("Player code not found");
		}
		// There was an error, check the debug logs
		// This could indicate an issue with RaidParty server
		else {
			Debug.Log ("Had a different error response from sdk: " + connectResponse);
		}

	}


	/**
	 * The player completes a new level
	 * Now you want to track an actual game event against the player
	 * */
	public void playerLevelUp() {

		// Attempt to track the player with the code they provide
		// raidPartySdk.trackEvent ("<Event ID>","<Event value e.g. if level 25, you can set to '25'>");
		int trackEventResponse = raidPartySdk.trackEvent ("1","25");

		Debug.Log ("raidPartySDK trackEvent response is: " + trackEventResponse);

		// If we get a success 201 response, the event was tracked successfully
		// Your game doesn't need to inform the player, the RaidParty system
		// will issue rewards automatically based on settings
		if (trackEventResponse == 201) {
			Debug.Log ("Player event was tracked successfully in RaidParty");
		} 
		// There was an error, check the debug logs
		// Most likely cause is the event doesn't exist in RaidParty
		// Please check the event ID you recorded
		else if (trackEventResponse == 404) {
			Debug.Log ("This event or player was not found.");
		}
		// There was an error, check the debug logs
		// This could indicate an issue with RaidParty server
		else {
			Debug.Log ("Had a different error response from sdk: " + trackEventResponse);
		}

	}
}
