using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Stack Size Controller", "Waizujin", 1.4)]
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
    }
}
