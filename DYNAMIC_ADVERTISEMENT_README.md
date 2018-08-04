
# Unity SDK for game developers

  

Complete C# library project for creating SDK.

  
## Dynamic Advertising Setup

  

1. In Unity3d editor, create new Render Texture in the project area (right click -> create -> Render Texture). Call it 'VideoPlayerTexture'

2. Set size 1270 x 800

3. Now add the DynamicAdvert.cs script to the game object that needs to update an advert (e.g. Billboard, Vending Machine)

4. Drag the VideoPlayerTexture onto the gameobject you added the DynamicAdvert.cs script to. Make sure Emission is set on in the VideoPlayerTexture.

5. Now add a new gameobject, with Video Player. Set render mode to Render Texture, and attach the  VideoPlayerTexture to target texture.

6. Go back to the DynamicAdvert.cs gameobject and attach this Video Player object to the Video Player field

7. Set the asset ID to what RaidParty provided you

8. Set game ID to what RaidParty provided you



## TODO

1. Set the video to play on the render texture that is applied to the gameobject, instead of having to manually create unique textures for each gameobject
2. If no video is received, how to display the received image instead?
3. If no video or image is received, how to display blank image?
4. How can we update a vending machine image?
5. How could we update a car in-game?
6. Package it into an SDK Library (.dll)
7. Make it work on more interesting game objects such as the players character (e.g. tank)
8. Include game analytics and target to certain locations (player physical locations e.g. Iran or England)
9. Track game object vision time i.e. how long has the advert been actually displayed to a player?
10. Optimise the library so that web and socket requests are limited, and doesn't lag the game or cause games to crash
11. Make it work on multi-platform e.g. 2D and 3D games