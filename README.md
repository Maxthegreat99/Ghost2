# Ghost2
A plugin for TShock that allows admins to make themselves or players invisible to others.

**NOTE: consider twice before giving this effect to your players as it may give them n unnfair advantage in certain circumstances**

- Originally made by [DannyDan77](https://github.com/DannyDan77)
- Updated for 1.4 by [moisterrific](https://github.com/DannyDan77)
- Updated to 1.4.4.9 by Maxthegreat99

## Original plugin description
https://github.com/DannyDan77/Ghost

A plugin for TShock that allows admins to become invisible to their players.

What is actually happening though, is far more complicated. This kind of invisibilty requires no buffs, and is undetectable by people with modded clients (mostly). You can also chat, use commands, whisper people, and you won't appear on the player list all at the same time while using this plugin. It is toggleable by using the "/ghost" command and having the "ghost.ghost" permission.


## User Instructions
- type `/ghost` to toggle it on or off for yourself.
- to give the invisible effect to other players type a second parameter like so: `/ghost "Max The Great"`
- May spook unsuspecting players, use at your own risk!

## Known Issues
This plugin works for the most part, but there are a few caveats:

- Ghost will automatically turn off if you use an item (such as magic mirror) or command (such as /home) to return to your spawnpoint
- If you wander too far from the original point of where you enabled ghost, chunks of the world will fail to load (client side only, visible to only you). This can be fixed by disabling and re-enabling ghost. 
- If you disable ghost in the vicinity of other players, your character appearance will not immediately load and you will look kinda like the default character you see in character creation menu (only visible to other players).

## Notes
- While ghosted, you will be completely invisible and invincible in PvP, however regular mobs can still damage you (even your attacks will be invisible!).
- While ghosted, regular mobs will not spawn around you unless other non-ghosted players are nearby. 
- If you are ghosted in a boss fight, if all other players are dead the boss will despawn.
- You will appear completely invisible and not be listed under the `/who` command as well as being unlisted in the server console.
- You can still chat with the rest of the players as usual.
- You can teleport to other players, however chunks of your world will not fully load.
- Your character will not show up in the online players/PvP grid style GUI (Mobile).
