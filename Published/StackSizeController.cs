/******************************************************************************
* Version 1.5 Changelog
*** Added Updater Support - http://oxidemod.org/plugins/updater.681/
*** Added a /stack command allowing you to change the stack size of items on the go.
*** Added a /stackall command allowing you to change the stack size of all items on the go. (Except Water & Salt Water which is always the max integer value.)
******************************************************************************/

using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Stack Size Controller", "Waizujin", 1.5, ResourceId = 1185)]
    [Description("Allows you to set the max stack size of every item.")]
    public class StackSizeController : RustPlugin
    {
		protected override void LoadDefaultConfig()
        {
			PrintWarning("Creating a new configuration file.");

			var gameObjectArray = FileSystem.LoadAll<GameObject>("Assets/", ".item");
			var itemList = gameObjectArray.Select(x => x.GetComponent<ItemDefinition>()).Where(x => x != null).ToList();

			foreach (var item in itemList)
			{
				if (item.condition.enabled && item.condition.max > 0) { continue; }

				Config[item.displayName.english] = item.stackable;
			}
		}

        void OnServerInitialized()
        {
            permission.RegisterPermission("canChangeStackSize", this);

			var dirty = false;
			var itemList = SingletonComponent<ItemManager>.Instance.itemList;

			foreach (var item in itemList)
			{
				if (item.condition.enabled && item.condition.max > 0) { continue; }

				if (Config[item.displayName.english] == null)
				{
					Config[item.displayName.english] = item.stackable;
					dirty = true;
				}

				item.stackable = (int)Config[item.displayName.english];
			}

			if (!dirty) { return; }

			PrintWarning("Updating configuration file with new values.");
			SaveConfig();
		}

        [ChatCommand("stack")]
        private void StackCommand(BasePlayer player, string command, string[] args)
        {
            int stackAmount = 0;

            if (!hasPermission(player, "canChangeStackSize"))
			{
				SendReply(player, "You don't have permission to use this command.");

				return;
			}

			if (args.Length <= 1)
			{
                SendReply(player, "Syntax Error: Requires 2 arguments. Syntax Example: /stack ammo_rocket_hv 64 (Use shortname)");

				return;
			}

            if (int.TryParse(args[1], out stackAmount) == false)
            {
                SendReply(player, "Syntax Error: Stack Amount is not a number. Syntax Example: /stack ammo_rocket_hv 64 (Use shortname)");

                return;
            }

            List<ItemDefinition> items = SingletonComponent<ItemManager>.Instance.itemList.FindAll(x => x.shortname.Contains(args[0]));

            if (items[0].shortname == args[0])
            {
                if (items[0].condition.enabled && items[0].condition.max > 0) { return; }

                Config[items[0].displayName.english] = Convert.ToInt32(stackAmount);
                items[0].stackable = Convert.ToInt32(stackAmount);

                SaveConfig();

                SendReply(player, "Updated Stack Size for " + items[0].displayName.english + " (" + items[0].shortname + ") to " + stackAmount + ".");
            }
            else
            {
                string results = "";
                foreach (ItemDefinition item in items)
                {
                    if (results == "")
                    {
                        results = item.shortname;

                        continue;
                    }

                    results = results + ", " + item.shortname;
                }

                SendReply(player, "Did you mean one of these? " + results + " ?");
            }
        }

        [ChatCommand("stackall")]
        private void StackAllCommand(BasePlayer player, string command, string[] args)
        {
            if (!hasPermission(player, "canChangeStackSize"))
			{
				SendReply(player, "You don't have permission to use this command.");

				return;
			}

			if (args.Length == 0)
			{
                SendReply(player, "Syntax Error: Requires 1 argument. Syntax Example: /stackall 65000");

				return;
			}

            var itemList = SingletonComponent<ItemManager>.Instance.itemList;

			foreach (var item in itemList)
			{
                if (item.displayName.english.ToString() == "Salt Water" ||
                item.displayName.english.ToString() == "Water") { continue; }

				Config[item.displayName.english] = Convert.ToInt32(args[0]);
				item.stackable = Convert.ToInt32(args[0]);
			}

            SaveConfig();

            SendReply(player, "The Stack Size of all stackable items has been set to " + args[0]);
        }

        bool hasPermission(BasePlayer player, string perm)
        {
            if (player.net.connection.authLevel > 1)
            {
                return true;
            }

            return permission.UserHasPermission(player.userID.ToString(), perm);
        }
    }
}
