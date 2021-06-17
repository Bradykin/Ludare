# Ludare
Platformer Prototype for Ludare Games

How to build prototype:
1) Load game in Unity Hub on Unity version 2020.3.11f1
2) Go to File -> Build Settings
3) Select Build and Run, and navigate to Ludare Games Test -> Builds

How to play prototype:
Play from Build) Use the above steps to build prototype and then run the .exe of the build from the filepath Ludare Games Test -> Builds - > Ludare Games Test (Application)
Play from Editor) Open the game through the Unity hub, switch to the Level Select Scene, and click Play.

Level Designer Instructions:
- Level objects can be added to the level from the Assets -> Prefabs path. 
- Example hazards and platforms are there but you can resize either of them and make new ones of different sizes, so long as you resize both the sprite and the box collider
- Each level needs a Player, a Flag, a Level Controller, and a Canvas, all of which are currently set up in Level 3 for you, and can be copy pasted into any new level.
- Enemies can have their wander behaviour adjusted by adjusting the serialized variables on the Enemy prefab.
- Flags point to a next level through a Serialized value on them. You can also have multiple flags if you want a level to be able to send the player to multiple other destinations.
- No specific design has been set out for this third level. It includes examples of various world assets to be able to see how they are used.

Project Overview:
- Each scene represents its own level, with the exception of the level select scene.
- This project uses a Singleton system shown with the LevelController, where it will save the first instance of the object that is loaded and destroy others. These singletons should ideally be put in each scene so the game has access to them regardless what scene you load the game from, for ease of testing.
- The LevelController Singleton is responsible for tracking player score and maintaining resettable objects in a level: The Player, Enemies, and Coins. Any object that extends the IReset interface should call SubscribeResettable and UnsubscribeResettable to communicate with the LevelController, and implement whatever behaviour would be needed to reset their functionality in the functions from the interface.
- The camera follows the player and is attached to them. The player is pre-placed in each level at whatever starting location you want them to spawn at.
- In this game, you can double jump, and your double jump is reset every time you collect a coin, allowing you to use coins to jump across longer distances.
- Interactions between the Player and other objects are handled in the Player's OnTriggerEnter and OnTriggerStay functions. These check the type of object collided with and call corresponding behaviour. In the process to un-prototype this project, these could be cleaned up and improved on, particularly the interactions with platforms.
- Player input is handled through an InputController that called events that the PlayerController subscribes to.


External Assets:
- Most objects in the game are prototyped as simple shapes for ease of editing and resizing.
- Parallax background came from opengameart: https://opengameart.org/content/3-parallax-backgrounds
- Sound effects came from freesound: 
	- https://freesound.org/people/DaveJf/sounds/575611/
	- https://freesound.org/people/jacksonacademyashmore/sounds/414209/
	- https://freesound.org/people/ProjectsU012/sounds/341695/
	- https://freesound.org/people/MAJ061785/sounds/85539/