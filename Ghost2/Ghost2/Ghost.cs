using System;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using Microsoft.Xna.Framework;

namespace Ghost
{
    [ApiVersion(2, 1)]
    public class Ghost : TerrariaPlugin
    {
        public override string Name => "Ghost";

        public override string Author => "SirApples, updated by moisterrific + Maxthegreat99";

        public override string Description => "A plugin that allows admins to become completely invisible to players.";

        public override Version Version => new Version(2, 1);

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("ghost.ghost", OnGhost, "ghost", "vanish"));
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


            int i = Projectile.NewProjectile(spawnSource: new Terraria.DataStructures.EntitySource_DebugCommand(), playerAffected.LastNetPosition.X, playerAffected.LastNetPosition.Y, Vector2.Zero.X, Vector2.Zero.Y, 0, 0, 0, 16, 0, 0);//Projectile.NewProjectile(args.Player.LastNetPosition.X, args.Player.LastNetPosition.Y, Vector2.Zero.X, Vector2.Zero.Y, 0, 0, 0, 16, 0, 0);
            Main.projectile[i].timeLeft = 0;

            NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, i);
            TplayerAffected.active = !TplayerAffected.active;
            NetMessage.SendData((int)PacketTypes.PlayerActive, -1, playerAffected.Index, null, playerAffected.Index, TplayerAffected.active.GetHashCode());

            if (args.TPlayer.active)
            {
                NetMessage.SendData((int)PacketTypes.PlayerInfo, -1, playerAffected.Index, null, playerAffected.Index);
                NetMessage.SendData((int)PacketTypes.PlayerUpdate, -1, playerAffected.Index, null, playerAffected.Index);
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
