using System;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;
using Terraria.Social.Base;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Net;

namespace Ghost
{
    [ApiVersion(2, 1)]
    public class Ghost : TerrariaPlugin
    {
        public override string Name => "Ghost";

        public override string Author => "SirApples, updated by moisterrific + Maxthegreat99";

        public override string Description => "A plugin that allows admins to become completely invisible to players.";

        public override Version Version => new Version(2, 1, 1);

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("ghost.ghost", OnGhost, "ghost", "vanish"));
        }
        private void ShowPlayerTo(TSPlayer target, TSPlayer sender)
        {

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


            int i = Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(), 
                                             playerAffected.LastNetPosition.X, playerAffected.LastNetPosition.Y,
                                             Vector2.Zero.X, Vector2.Zero.Y, 0, 0, 0, 16, 0, 0);
            Main.projectile[i].timeLeft = 0;

            NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, i);
            TplayerAffected.active = !TplayerAffected.active;
            NetMessage.SendData((int)PacketTypes.PlayerActive, -1, playerAffected.Index, null, playerAffected.Index, TplayerAffected.active.GetHashCode());

            
            if (TplayerAffected.active)
            {
                NetMessage.SendData((int)PacketTypes.PlayerInfo, -1, playerAffected.Index, null, playerAffected.Index);
                NetMessage.SendData((int)PacketTypes.PlayerUpdate, -1, playerAffected.Index, null, playerAffected.Index);
                ShowPlayerTo(TSPlayer.All, playerAffected);
            }

            playerAffected.SendSuccessMessage($"{(TplayerAffected.active ? "Dis" : "En")}abled Ghost.");
            args.Player.SendSuccessMessage($"Player {playerAffected.Name} has now Ghost {(TplayerAffected.active ? "Dis" : "En")}abled.");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

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
