This readme doubles as a documentation. While I used unity before, this is my first fully 3d project so I apologise in advance if you find things that are less orthodox.

In addition to adding several scripts, I've made tiny modifications to assets including adding events to death functions and adjusting colour for the attack distance texture.

While coding the scripts I used a centralized game logic where a main script handles interactions between objects and objects handle only things that concern themselves. I know that this is not always the first choice in unity but I find it simpler to read, understand and modify.

In order to instantiate game objects I generated dud versions to editor and cloned them with Instantiate objects. And I connected things directly instead of searching for them on start. Both of these choices are safety measures to Unity's unpredictable initialization order of objects.

I have no experience about lighting so I just played with it until I'm happy with what I see and I did not try to mimic the shadow patterns from the video, otherwise I tried to make it look as much like the video as I can.

I made two alterations to the given briefing. First is, instead of having coins appear over time, I made it so that coins disappear when touched or are too far and reappear randomly around the character, thereby keeping the total number fixed. And the other is, I made the boomerang come back to the player, and kill all enemies it touches along its flight.

The scripts have their own explanations as comments but while reading, starting from Main may make it easier to understand whats happening.

Have a nice day.