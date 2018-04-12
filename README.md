# Unity SDK for game developers

Complete C# library project for creating SDK.

## Getting Started - Generating dll

1. Open the projet in C# IDE like Monodevelop or Visual Code.
2. Locate MyClass.cs
3. Build the project.
4. Locate raidParty\_SDK.dll in ./raidParty\_SDK/bin/Debug/ . 

### Prerequisites

C# Development Envrionment

### How to use SDK

1. Unity: Select Import new asset.

2. Locate raidParty_SDK.dll and import. 

3. Now in internal C# scripts in Unity:

 ```
 Using raidParty_SDK;
 ```
4. Create the raidPartyLib class Instance and pass the required params.
 
 ```
 raidPartyLib instance = new raidPartyLib("<Game Public Key>", "<Game Private Key>")
 ```

__raidPartyLib instance is now ready to use.__

### Track Player

Players' login activity can be tracked using 'trackPlayer' method of raidPartyLib. Pass in the player id recieved from players to sync with the backend. 

* playerId

```
instance.trackPlayer(<"playerId">);
```

### Reward Player Event

Events can be generated using 'rewardPlayerEvent' method of raidPartyLib.
This method requires two arguments:

 * event name

 * event description

 * event value (reward value in terms of how many force tokens) (Optional)

```
instance.rewardPlayerEvent("<event name>", "<event description>", "<event Value>");
```
