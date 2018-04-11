# Unity SDK for game developers

Complete C# library project for creating SDK.

## Getting Started - Generating dll

1. Open the projet in C# IDE like Monodevelop or Visual Code.
2. Locate MyClass.cs
3. Build the project.
4. Locate triforce\_SDK.dll in ./triforce\_SDK/bin/Debug/ . 

### Prerequisites

C# Development Envrionment

### How to use SDK

1. Unity: Select Import new asset.

2. Locate triforce_SDK.dll and import. 

3. Now in internal C# scripts in Unity:

 ```
 Using triforce_SDK;
 ```
4. Create the TriforceLib class Instance and pass the required params.
 
 ```
 TriforceLib instance = new TriforceLib("<Player 
 Email Address>", "<Game Public Key>", "<Game 
 Private Key>")
 ```

__TriforceLib instance is now ready to use.__

### Track Player

Players can be tracked using 'trackPlayer' method of TriforceLib.

```
instance.trackPlayer();
```

### Reward Player Event

Event for rewarding player can be generated using 'rewardPlayerEvent' method of TriforceLib.
This method requires two arguments:

 * event description

 * event value (reward value in terms of how many force tokens)

```
instance.rewardPlayerEvent("<event description>", "<event Value>");
```
