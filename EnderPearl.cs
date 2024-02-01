using CommandSystem;
using Exiled.API.Features;
using System;

namespace EnderPearl
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EnderPearl : ICommand
    {
        public string Command { get; } = "EnderPearl";

        public string[] Aliases { get; } = { "enderpearl" };

        public string Description { get; } = "Why not?";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission(PlayerPermissions.GivingItems))
            {
                try
                {
                    if (arguments.Count < 2)
                    {
                        response = "Usage: enderpearl [player id / nick] [on/off]";
                        return false;
                    }

                    if (arguments.At(1) == "on")
                    {
                        Main.ItemTeleportPlayers.Add(Player.Get(arguments.At(0)));
                    }
                    else
                    {
                        Main.ItemTeleportPlayers.Remove(Player.Get(arguments.At(0)));
                    }

                    response = "Sucess!";
                    return true;
                }
                catch (Exception err)
                {
                    Log.Error(err);
                    response = "An error has ocurred! " + err.ToString();
                    return false;
                }
            }
            else
            {
                response = "You do not have permissions to execute this command! You need GivinItems permission";
                return false;
            }
        }
    }
}