using System;
using System.Collections;
using UnityEngine;
using BestHTTP;
using BestHTTP.SocketIO;
using System.Collections.Generic;
using LitJson;
using UnityEngine.Video;

public class DynamicAdvert : MonoBehaviour
{
	public VideoPlayer videoPlayer;
	public string assetId;
	public string gameId;
	public bool playAudio;
	public bool isVideo = false;
	public bool isTexture = false;
	public bool test = true;

	SocketManager manager;
    private string address = "";

    void OnEnable()
    {
    	//BestHTTP.HTTPManager.Logger.Level = BestHTTP.Logger.Loglevels.All;

    	if(!test){
    		address = "https://hub.staging.raidparty.io/socket.io/";
    	}else{
    		address = "http://localhost:1338/socket.io/";
    	}

    	videoPlayer.errorReceived += videoErrorReceived;

    	if(!playAudio){
    		videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
    	}

		// Change an option to show how it should be done
        SocketOptions options = new SocketOptions();
        options.AutoConnect = false;
        options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;
        options.QueryParamsOnlyForHandshake = false;
        options.AdditionalQueryParams = new PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string>();
        options.AdditionalQueryParams.Add("__sails_io_sdk_version", "0.13.8"); // !REQUIRED - DO NOT REMOVE

        // Create the Socket.IO manager
        manager = new SocketManager(new Uri(address), options);
        manager.Encoder = new BestHTTP.SocketIO.JsonEncoders.LitJsonEncoder();

        // Set up custom chat events
        manager.Socket.On("connect", OnConnect);

        // Socket send 'advert' message 
        // New advert should be displayed on this game object
        manager.Socket.On("advert", OnAdvert);

        // Set SocketOptions' AutoConnect to false, so we have to call it manually.
        manager.Open();
    }


    void OnDisable(){
		videoPlayer.errorReceived -= videoErrorReceived;
    }


    void OnConnect(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Dyanmic Advert connection established");

	    var methodParams = new {url = "/sdk/advert/connect/" + gameId + "/" + assetId};

	    manager.Socket.Emit("get", OnConnectAdvert, methodParams);
	}


	void OnConnectAdvert(Socket socket, Packet packet, params object[] args)
	{
    	Debug.Log("Dyanmic Advert channel opened");
	}


	void OnAdvert(Socket socket, Packet packet, params object[] args)
	{
		try
		{
			Dictionary<string, object> obj = args[0] as Dictionary<string, object>;

			var advertId = obj["advertId"];
			var resourceUrlHd = obj["resourceUrlHd"].ToString();
			var resourceUrlSd = obj["resourceUrlSd"].ToString();
			var resourceUrlImg = obj["resourceUrlImg"].ToString();
			var width = obj["width"];
			var height = obj["height"];
			var link = obj["link"].ToString();


			// Game Object is a video screen
			if(isVideo){
				if(resourceUrlSd != ""){
					Debug.Log("resource video SD is: " + resourceUrlSd);
					this.RenderAdvertVideo(resourceUrlSd);
				}else if(resourceUrlHd != ""){
					Debug.Log("resource video HD is: " + resourceUrlHd);
					this.RenderAdvertVideo(resourceUrlHd);
				}
			}

			// Game object is a texture
			if(isTexture){
				if(resourceUrlImg != ""){
					Debug.Log("resource image is: " + resourceUrlImg);
					//this.RenderAdvertImage(resourceUrlImg,width,height);
				}
			}

		}
		catch(Exception ex)
		{
			Debug.LogError("there was an error: " + ex);
		}

	}


/**
	IEnumerator RenderAdvertImage(string url, float width, float height)
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }*/


    /**
    *  Received a call to 
    */
    public void RenderAdvertVideo(string url){
    	if(videoPlayer.url != url){
        	videoPlayer.url = url;
        	videoPlayer.isLooping = true;
        	videoPlayer.playOnAwake = true;
	        //videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
	        //videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
	        //videoPlayer.targetMaterialProperty = "_MainTex";
	        videoPlayer.Prepare();
	        videoPlayer.Play();
        }
    }

    private void videoErrorReceived(VideoPlayer v, string msg){
    	Debug.Log("Video Player error: " + msg);
    }

}
