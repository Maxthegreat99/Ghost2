using System;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;
using System.Collections.Generic;
using System.Linq;

namespace Ghost
{
    [ApiVersion(2, 1)]
    public class Ghost : TerrariaPlugin
    {
        public override string Name => "Ghost";

        public override string Author => "SirApples, updated by moisterrific + Maxthegreat99";

        public override string Description => "A plugin that allows admins to become completely invisible to players.";

        public static class Permissions
        {
            public const string ghost = "ghost.ghost";

            public const string hardGhost = "ghost.hardghost";

            public const string seeGhosts = "ghost.see";

            public const string silentJoin = "ghost.silentjoin";

            public const string silentLeave = "ghost.silentleave";

            public const string ghostOnJoin = "ghost.ghostonjoin";

            public const string hghostOnJoin = "ghost.hghostonjoin";
        }

        public override Version Version => new Version(2, 3);

        public List<int> playersGhosted = new();

        // Key: index Value: player's last position
        public Dictionary<int, Vector2> hardGhostedPlayersLastPosition = new();

        private const int MIN_DIST_TO_UPDATE_TILES = 15;

        private const int SIZE_OF_TILE_UPDATE = 80;

        public override void Initialize()
        {
            ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnJoin);
            ServerApi.Hooks.GameUpdate.Register(this, OnUpdate);

            Commands.ChatCommands.Add(new Command(Permissions.ghost, OnGhost, "ghost", "vanish")
            {
                HelpText = "Usage: '/ghost (optional)<player>'" +
                           "\nMakes you invisible from anyone without the 'ghost.see' permission. This means the server itself can still see you." +
                           string.Format("\n[c/{0}:NOTE: While you have ghost enabled the server tries to re-ghost you each time someone joins which is why you can get teleported to where you are when that happens.]", Color.DarkRed.Hex3())
            });
            Commands.ChatCommands.Add(new Command(Permissions.hardGhost, OnHardGhost, "hardghost", "hardvanish", "hghost", "hvanish")
            {
                HelpText = "Usage: '/hghost (optional)<player>'" +
                "\nMakes you completely invisible from everyone(including the server/people with the 'ghost.see' permission). This version of ghost does not re-ghosts you when players join" +
                string.Format("\n[c/{0}:NOTE: While you have hard ghost enabled the plugin will manually send you map data which is why you might experience some visual bugs/lag/desync when it comes to the world itself.]", Color.DarkRed.Hex3())
            });
        }
        void OnGhost(CommandArgs args)
        {
            if (!args.Player.RealPlayer && args.Parameters.Count < 1)
            {
                args.Player.SendErrorMessage("You can't ghost the console!");
                return;
            }
            Player TplayerAffected = args.TPlayer;
            TSPlayer playerAffected = args.Player;
            if (args.Parameters.Count > 0)
            {
                if (TSPlayer.FindByNameOrID(args.Parameters[0]).Count < 1)
                {
                    args.Player.SendErrorMessage("Player not found!");
                    return;
                }
                playerAffected = TSPlayer.FindByNameOrID(args.Parameters[0]).FirstOrDefault();
                TplayerAffected = playerAffected.TPlayer;
                
            }

            if (!TplayerAffected.active)
            {
                args.Player.SendErrorMessage("target already has Hard Ghost enabled(or is inactive)! use /hghost to disable Hard Ghost.");
                return;
            }


            //is player not ghosted?
            if (!playersGhosted.Contains(playerAffected.TPlayer.whoAmI))
            {
                playersGhosted.Add(playerAffected.TPlayer.whoAmI);

                utils.NormalGhostPlayer(playerAffected, TplayerAffected);
            }
            else
            {
                playersGhosted.Remove(playerAffected.TPlayer.whoAmI);

                playerAffected.Teleport(playerAffected.TPlayer.position.X, playerAffected.TPlayer.position.Y);

                NetMessage.SendData((int)PacketTypes.PlayerActive, -1, playerAffected.Index, null, playerAffected.Index, TplayerAffected.active.GetHashCode());
                utils.ShowPlayerTo(TSPlayer.All, playerAffected);
            }

            playerAffected.SendSuccessMessage($"{(!playersGhosted.Contains(playerAffected.TPlayer.whoAmI) ? "Dis" : "En")}abled Ghost.");
            args.Player.SendSuccessMessage($"Player {playerAffected.Name} has now Ghost {(!playersGhosted.Contains(playerAffected.TPlayer.whoAmI) ? "Dis" : "En")}abled.");

        }

        void OnHardGhost(CommandArgs args)
        {

            if (!args.Player.RealPlayer && args.Parameters.Count < 1)
            {
                args.Player.SendErrorMessage("You can't ghost the console!");
                return;
            }

            Player TplayerAffected = args.TPlayer;
            TSPlayer playerAffected = args.Player;
            if (args.Parameters.Count > 0)
            {
                if (TSPlayer.FindByNameOrID(args.Parameters[0]).Count < 1)
                {
                    args.Player.SendErrorMessage("Player not found!");
                    return;
                }
                playerAffected = TSPlayer.FindByNameOrID(args.Parameters[0]).FirstOrDefault();
                TplayerAffected = playerAffected.TPlayer;

            }

            if (playersGhosted.Contains(playerAffected.TPlayer.whoAmI))
            {
                args.Player.SendErrorMessage("target already has Ghost enabled! use /ghost to disable Ghost.");
                return;
            }


            utils.GhostPlayer(playerAffected, TplayerAffected);
            if(!hardGhostedPlayersLastPosition.ContainsKey(playerAffected.Index))
                hardGhostedPlayersLastPosition.Add(playerAffected.Index, TplayerAffected.position);
            
            if (TplayerAffected.active)
            {
                hardGhostedPlayersLastPosition.Remove(playerAffected.Index);
                playerAffected.Teleport(playerAffected.TPlayer.position.X, playerAffected.TPlayer.position.Y);
                utils.ShowPlayerTo(TSPlayer.All, playerAffected);
            }

            playerAffected.SendSuccessMessage($"{(TplayerAffected.active ? "Dis" : "En")}abled Hard Ghost.");
            args.Player.SendSuccessMessage($"Player {playerAffected.Name} has now Hard Ghost {(TplayerAffected.active ? "Dis" : "En")}abled.");
        }
        
        private void OnServerLeave(LeaveEventArgs args)
        {
            if (TShock.Players[args.Who] == null)
                return;

            var player = TShock.Players[args.Who];

            if (playersGhosted.Contains(args.Who) )
                playersGhosted.Remove(args.Who);

            if (hardGhostedPlayersLastPosition.ContainsKey(args.Who))
                hardGhostedPlayersLastPosition.Remove(args.Who);

            if(player.HasPermission(Permissions.silentLeave))
                player.SilentKickInProgress = true;
        }

        private void OnJoin(GreetPlayerEventArgs args)
        {
            if (args.Handled || TShock.Players[args.Who] == null)
                return;

            var player = TShock.Players[args.Who];

            if (player.HasPermission(Permissions.silentJoin))
                player.SilentJoinInProgress = true;

            if (player.HasPermission(Permissions.hghostOnJoin))
            {
                Commands.HandleCommand(player, "/hghost");
                return;
            }
            if (player.HasPermission(Permissions.ghostOnJoin))
            {
                Commands.HandleCommand(player, "/ghost");
                return;
            }

            if (playersGhosted.Count == 0 || player.HasPermission(Permissions.seeGhosts))
                return;

            foreach(int playerIndex in playersGhosted)
            {
                TSPlayer playerAffected = TShock.Players[playerIndex];
                Player TplayerAffected = playerAffected.TPlayer;

                utils.NormalGhostPlayer(playerAffected, TplayerAffected);
            }

        }

        private void OnUpdate(EventArgs args)
        {
            if (hardGhostedPlayersLastPosition.Count == 0)
                return;

            foreach(KeyValuePair<int, Vector2> kvp in hardGhostedPlayersLastPosition)
            {
                var player = TShock.Players[kvp.Key];

                if (player.TPlayer.position.DistanceSQ(kvp.Value) <= MIN_DIST_TO_UPDATE_TILES * MIN_DIST_TO_UPDATE_TILES)
                    continue;
                
                var tmp = new Dictionary<int, Vector2>(hardGhostedPlayersLastPosition);
                tmp[kvp.Key] = player.TPlayer.position;

                hardGhostedPlayersLastPosition = new Dictionary<int, Vector2>(tmp);

                player.SendTileSquareCentered(player.TileX, player.TileY, SIZE_OF_TILE_UPDATE);
            }
        }

        public static class utils
        {
            public static void ShowPlayerTo(TSPlayer target, TSPlayer sender)
            {
                target.SendData(PacketTypes.PlayerInfo, "", sender.Index);
                target.SendData(PacketTypes.PlayerUpdate, "", sender.Index);

                float slot = 0f;
                for (int k = 0; k < NetItem.InventorySlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].inventory[k].Name, sender.Index, slot, (float)Main.player[sender.Index].inventory[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.ArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.DyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].dye[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.MiscEquipSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].miscEquips[k].Name, sender.Index, slot, (float)Main.player[sender.Index].miscEquips[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.MiscDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].miscDyes[k].Name, sender.Index, slot, (float)Main.player[sender.Index].miscDyes[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.PiggySlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank.item[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.SafeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank2.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank2.item[k].prefix);
                    slot++;
                }
                target.SendData((PacketTypes)5, Main.player[sender.Index].trashItem.Name, sender.Index, slot++, (float)Main.player[sender.Index].trashItem.prefix);
                for (int k = 0; k < NetItem.ForgeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank3.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank3.item[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.VoidSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank4.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank4.item[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[0].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[0].Armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[0].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[0].Dye[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[1].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[1].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Dye[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[2].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[2].Armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[1].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[2].Dye[k].prefix);
                    slot++;
                }


                target.SendData((PacketTypes)4, sender.Name, sender.Index, 0f, 0f, 0f, 0);
                target.SendData((PacketTypes)42, "", sender.Index, 0f, 0f, 0f, 0);
                target.SendData((PacketTypes)16, "", sender.Index, 0f, 0f, 0f, 0);

                slot = 0f;
                for (int k = 0; k < NetItem.InventorySlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].inventory[k].Name, sender.Index, slot, (float)Main.player[sender.Index].inventory[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.ArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.DyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].dye[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.MiscEquipSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].miscEquips[k].Name, sender.Index, slot, (float)Main.player[sender.Index].miscEquips[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.MiscDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].miscDyes[k].Name, sender.Index, slot, (float)Main.player[sender.Index].miscDyes[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.PiggySlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank.item[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.SafeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank2.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank2.item[k].prefix);
                    slot++;
                }
                target.SendData((PacketTypes)5, Main.player[sender.Index].trashItem.Name, sender.Index, slot++, (float)Main.player[sender.Index].trashItem.prefix);
                for (int k = 0; k < NetItem.ForgeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank3.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank3.item[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.VoidSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].bank4.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank4.item[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[0].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[0].Armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[0].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[0].Dye[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[1].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[1].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Dye[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[2].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[2].Armor[k].prefix);
                    slot++;
                }
                for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
                {
                    target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[2].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[2].Dye[k].prefix);
                    slot++;
                }

                target.SendData((PacketTypes)4, sender.Name, sender.Index, 0f, 0f, 0f, 0);
                target.SendData((PacketTypes)42, "", sender.Index, 0f, 0f, 0f, 0);
                target.SendData((PacketTypes)16, "", sender.Index, 0f, 0f, 0f, 0);

                for (int k = 0; k < Player.maxBuffs; k++)
                {
                    sender.TPlayer.buffType[k] = 0;
                }

                target.SendData((PacketTypes)50, "", sender.Index, 0f, 0f, 0f, 0);

                target.SendData((PacketTypes)76, "", sender.Index);

                target.SendData((PacketTypes)39, "", 400);

                if (Main.GameModeInfo.IsJourneyMode)
                {
                    var sacrificedItems = TShock.ResearchDatastore.GetSacrificedItems();
                    for (int i = 0; i < ItemID.Count; i++)
                    {
                        var amount = 0;
                        if (sacrificedItems.ContainsKey(i))
                        {
                            amount = sacrificedItems[i];
                        }

                        var response = NetCreativeUnlocksModule.SerializeItemSacrifice(i, amount);
                        NetManager.Instance.SendToClient(response, sender.Index);
                    }
                }


            }
            public static void GhostPlayer(TSPlayer target, Player ttarget)
            {
                int i = Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(),
                                target.LastNetPosition.X, target.LastNetPosition.Y,
                                Vector2.Zero.X, Vector2.Zero.Y, 0, 0, 0, 16, 0, 0);
                Main.projectile[i].timeLeft = 0;

                NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, i);
                ttarget.active = !ttarget.active;
                NetMessage.SendData((int)PacketTypes.PlayerActive, -1, target.Index, null, target.Index, ttarget.active.GetHashCode());
            }

            public static void NormalGhostPlayer(TSPlayer playerAffected, Player TplayerAffected)
            {
                utils.GhostPlayer(playerAffected, TplayerAffected);

                TplayerAffected.active = true;

                //resets things like grapples which arent sent to the target
                playerAffected.Teleport(playerAffected.TPlayer.position.X, playerAffected.TPlayer.position.Y);

                foreach (TSPlayer player in TShock.Players)
                {

                    if (player == null || player.TPlayer.whoAmI == TplayerAffected.whoAmI)
                        continue;

                    if (player.HasPermission(Permissions.seeGhosts))
                    {
                        player.SendData(PacketTypes.PlayerActive, "", playerAffected.Index, TplayerAffected.active.GetHashCode());
                        utils.ShowPlayerTo(player, playerAffected);
                    }
                }
            } 
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnJoin);
                ServerApi.Hooks.GameUpdate.Deregister(this, OnUpdate);
            }
            base.Dispose(disposing);
        }

        public Ghost(Main game)
            : base(game)
        {
            //Order = 10;
        }
    }
}
