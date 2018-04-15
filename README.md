
# Unity SDK for game developers

  

Complete C# library project for creating SDK.

  

## Getting Started - Generating dll

  

1. Open the projet in C# IDE like Monodevelop or Visual Code.

2. Locate RaidPartyLib.cs

3. Build the project.

4. Locate raidParty\_SDK.dll in ./raidParty\_SDK/bin/Debug/ .

  

### Prerequisites

  

C# Development Envrionment

  

### How to use SDK

  

1. Unity: Select 'Assets' -> `Import new asset`.

  

2. Locate `raidParty_SDK.dll` and import into Unity.

  

3. Now in internal C# scripts in Unity:

  
```

using raidParty_SDK;

```

4. Create the raidPartyLib class Instance and pass the required params.

```

raidPartyLib raidParty = new raidPartyLib("<Game App ID>", "<Game App KEY>", "<bool testing?>")

```

  

__raidPartyLib instance is now ready to use in your game code.__

  

### Track Player

  

Players' login activity can be tracked using 'trackPlayer' method of raidPartyLib. Pass in the player id recieved from players to sync with the backend.

  

* raidPartyUid

  

```

raidParty.trackPlayer(<"raidPartyUid">);

```

  

### Track Event

  

Events can be generated using 'trackEvent' method of raidPartyLib.

This method requires two arguments:

  

* event ID
  

* event value (reward value in terms of how many force tokens) (Optional)

  

```

raidParty.trackEvent("<event ID>", "<event Value>");

```