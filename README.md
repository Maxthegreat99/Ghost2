# Ghost2
A plugin for TShock giving admins multiple stealthing abilties, such as becoming completely invisible to their players, silent joning and much more!

### NOTE: consider twice before giving these effects to your players as it may give them unfair advantages or even the ability to bypass anticheats

- Originally made by [DannyDan77](https://github.com/DannyDan77)
- Updated for `1.4` by [moisterrific](https://github.com/moisterrific)
- Updated to `1.4.4.9` by Maxthegreat99

## Original plugin description
https://github.com/DannyDan77/Ghost

A plugin for TShock that allows admins to become invisible to their players.

What is actually happening though, is far more complicated. This kind of invisibilty requires no buffs, and is undetectable by people with modded clients (mostly). You can also chat, use commands, whisper people, and you won't appear on the player list all at the same time while using this plugin. It is toggleable by using the "/ghost" command and having the "ghost.ghost" permission.

## How to Install
1. Put the .dll into the `\ServerPlugins\` folder.
2. Restart the server.
3. Give your desired group the the permissions defined in the configs folder.

## User Instructions
### Commands and Usage
- `/ghost` - toggles on/off the basic ghost mode for yourself that allows only the server and players with the `ghost.see` permission to see you.
- `/ghost {player name}` - gives the same effect to the targeted player.
- `/hghost` - toggles on/off hard ghost mode for yourself, this version of ghost allows you to be invisible to everyone, including players with the `ghost.see` perms and the server.
- `/hghost {player name}` - gives the same effect to the targeted player.
#### May spook unsuspecting players, use at your own risk!

### Permissions
- `ghost.ghost` - allows the player to use the `/ghost` command.
- `ghost.hardghost` - allows the player to use the `/hghost` command.
- `ghost.see` - allows the player to see `/ghost`ed players(does not work for players who have been `/hghost`ed)
- `ghost.silentleave` - disables leaving messages for the player.
- `ghost.silentjoin` - disables the joining messages for the player.
- `ghost.hghostonjoin` - hard ghosts the player upon joining.
- `ghost.ghostonjoin` - ghosts the player upon joning.

## Known Issues
This plugin works for the most part, but there are a few caveats:

- Ghost will automatically turn off if you use an item (such as magic mirror) or command (such as `/home`) to return to your spawnpoint
- If you wander too far from the original point of where you enabled ghost, chunks of the world will fail to load (client side only, visible to only you). This can be fixed by disabling and re-enabling ghost. 

## Notes
- While ghosted, you will be completely invisible and invincible in PvP, however regular mobs can still damage you (even your attacks will be invisible!).
- While ghosted, regular mobs will not spawn around you unless other non-ghosted players are nearby. 
- If you are hard ghosted in a boss fight, if all other players are dead the boss will despawn.
- You will appear completely invisible and not be listed under the `/who` command as well as being unlisted in the server console.(using `/hghost` only)
- You can still chat with the rest of the players as usual.
- You can teleport to other players, however chunks of your world will not fully load.
- Your character will not show up in the online players/PvP grid style GUI (Mobile).

## Forked repository
https://github.com/moisterrific/Ghost2
