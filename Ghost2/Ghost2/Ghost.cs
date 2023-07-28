using System;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;
using System.Collections.Generic;

namespace Ghost
{
    [ApiVersion(2, 1)]
    public class Ghost : TerrariaPlugin
    {
        public override string Name => "Ghost";

        public override string Author => "SirApples, updated by moisterrific + Maxthegreat99";

        public override string Description => "A plugin that allows admins to become completely invisible to players.";

        public override Version Version => new Version(2, 2);

        public List<int> playersGhosted = new();

        public override void Initialize()
        {
            ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnJoin);

            Commands.ChatCommands.Add(new Command("ghost.ghost", OnGhost, "ghost", "vanish"));
            Commands.ChatCommands.Add(new Command("ghost.hardghost", OnHardGhost, "hardghost", "hardvanish", "hghost", "hvanish"));
        }

        private void ShowPlayerTo(TSPlayer target, TSPlayer sender)
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
                target.SendData((PacketTypes)5,Main.player[sender.Index].bank2.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank2.item[k].prefix);
                slot++;
            }
            target.SendData((PacketTypes)5, Main.player[sender.Index].trashItem.Name, sender.Index, slot++, (float)Main.player[sender.Index].trashItem.prefix);
            for (int k = 0; k < NetItem.ForgeSlots; k++)
            {
                target.SendData((PacketTypes)5,Main.player[sender.Index].bank3.item[k].Name, sender.Index, slot, (float)Main.player[sender.Index].bank3.item[k].prefix);
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
                target.SendData((PacketTypes)5,Main.player[sender.Index].Loadouts[1].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Dye[k].prefix);
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
            target.SendData((PacketTypes)16, "",sender.Index, 0f, 0f, 0f, 0);

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
                target.SendData((PacketTypes)5,  Main.player[sender.Index].miscEquips[k].Name, sender.Index, slot, (float)Main.player[sender.Index].miscEquips[k].prefix);
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
            target.SendData((PacketTypes)5,Main.player[sender.Index].trashItem.Name, sender.Index, slot++, (float)Main.player[sender.Index].trashItem.prefix);
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
                target.SendData((PacketTypes)5,Main.player[sender.Index].Loadouts[1].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Armor[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
            {
                target.SendData((PacketTypes)5, Main.player[sender.Index].Loadouts[1].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[1].Dye[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.LoadoutArmorSlots; k++)
            {
                target.SendData((PacketTypes)5,Main.player[sender.Index].Loadouts[2].Armor[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[2].Armor[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.LoadoutDyeSlots; k++)
            {
                target.SendData((PacketTypes)5,Main.player[sender.Index].Loadouts[2].Dye[k].Name, sender.Index, slot, (float)Main.player[sender.Index].Loadouts[2].Dye[k].prefix);
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

            target.SendData((PacketTypes)76,"", sender.Index);

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
        void GhostPlayer(TSPlayer target, Player ttarget)
        {
            int i = Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(),
                            target.LastNetPosition.X, target.LastNetPosition.Y,
                            Vector2.Zero.X, Vector2.Zero.Y, 0, 0, 0, 16, 0, 0);
            Main.projectile[i].timeLeft = 0;

            NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, i);
            ttarget.active = !ttarget.active;
            NetMessage.SendData((int)PacketTypes.PlayerActive, -1, target.Index, null, target.Index, ttarget.active.GetHashCode());
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
                TplayerAffected = TSPlayer.FindByNameOrID(args.Parameters[0])[0].TPlayer;
                playerAffected = TSPlayer.FindByNameOrID(args.Parameters[0])[0];
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

                GhostPlayer(playerAffected, TplayerAffected);

                TplayerAffected.active = true;

                //resets things like grapples which arent sent to the target
                playerAffected.Teleport(playerAffected.TPlayer.position.X, playerAffected.TPlayer.position.Y);

                foreach (TSPlayer player in TShock.Players)
                {
                    
                    if (player == null || player.TPlayer.whoAmI == TplayerAffected.whoAmI)
                        continue;

                    if (player.HasPermission("ghost.see"))
                    {
                        player.SendData(PacketTypes.PlayerActive, "", playerAffected.Index, TplayerAffected.active.GetHashCode());
                        ShowPlayerTo(player, playerAffected);
                    }
                }
            }
            else
            {
                playersGhosted.Remove(playerAffected.TPlayer.whoAmI);

                playerAffected.Teleport(playerAffected.TPlayer.position.X, playerAffected.TPlayer.position.Y);

                NetMessage.SendData((int)PacketTypes.PlayerActive, -1, playerAffected.Index, null, playerAffected.Index, TplayerAffected.active.GetHashCode());
                ShowPlayerTo(TSPlayer.All, playerAffected);
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
                TplayerAffected = TSPlayer.FindByNameOrID(args.Parameters[0])[0].TPlayer;
                playerAffected = TSPlayer.FindByNameOrID(args.Parameters[0])[0];

            }

            if (playersGhosted.Contains(playerAffected.TPlayer.whoAmI))
            {
                args.Player.SendErrorMessage("target already has Ghost enabled! use /ghost to disable Ghost.");
                return;
            }


            GhostPlayer(playerAffected, TplayerAffected);

            
            if (TplayerAffected.active)
            {
                playerAffected.Teleport(playerAffected.TPlayer.position.X, playerAffected.TPlayer.position.Y);
                ShowPlayerTo(TSPlayer.All, playerAffected);
            }

            playerAffected.SendSuccessMessage($"{(TplayerAffected.active ? "Dis" : "En")}abled Hard Ghost.");
            args.Player.SendSuccessMessage($"Player {playerAffected.Name} has now Hard Ghost {(TplayerAffected.active ? "Dis" : "En")}abled.");
        }
        
        private void OnServerLeave(LeaveEventArgs args)
        {
            if (playersGhosted.Contains(args.Who) )
                playersGhosted.Remove(args.Who);

            if(TShock.Players[args.Who].HasPermission("ghost.silentleave"))
                TShock.Players[args.Who].SilentKickInProgress = true;
        }

        private void OnJoin(GreetPlayerEventArgs args)
        {
            if (TShock.Players[args.Who].HasPermission("ghost.silentjoin"))
                TShock.Players[args.Who].SilentJoinInProgress = true;

            if (TShock.Players[args.Who].HasPermission("ghost.hghostonjoin"))
            {
                Commands.HandleCommand(TShock.Players[args.Who], "/hghost");
                return;
            }
            if (TShock.Players[args.Who].HasPermission("ghost.ghostonjoin"))
            {
                Commands.HandleCommand(TShock.Players[args.Who], "/ghost");
                return;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnJoin);
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
