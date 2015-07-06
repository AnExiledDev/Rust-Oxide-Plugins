namespace Oxide.Plugins
{
    [Info("Better Gather Rate", "Waizujin", 1.0)]
    [Description("Allow you to change the gather rate of each individual program.")]
    public class BetterGatherRate : RustPlugin
    {
        protected override void LoadDefaultConfig()
        {
            PrintWarning("Creating a new configuration file.");
            Config.Clear();

            Config["wood"]              = 1;
            Config["sulfur_ore"]        = 1;
            Config["metal_ore"]         = 1;
            Config["stones"]            = 1;
            Config["fat_animal"]        = 1;
            Config["wolfmeat_raw"]      = 1;
            Config["cloth"]             = 1;
            Config["bone_fragments"]    = 1;

            SaveConfig();
        }

        void OnGather(ResourceDispenser dispenser, BaseEntity entity, Item item)
        {
            int GatherRate = 1;
            string ShortName = item.info.shortname;

            if(ShortName == "wood") {
                GatherRate = (int) Config["wood"];
            } else if(ShortName == "sulfur_ore") {
                GatherRate = (int) Config["sulfur_ore"];
            } else if(ShortName == "metal_ore") {
                GatherRate = (int) Config["metal_ore"];
            } else if(ShortName == "stones") {
                GatherRate = (int) Config["stones"];
            } else if(ShortName == "fat_animal") {
                GatherRate = (int) Config["fat_animal"];
            } else if(ShortName == "wolfmeat_raw") {
                GatherRate = (int) Config["wolfmeat_raw"];
            } else if(ShortName == "cloth") {
                GatherRate = (int) Config["cloth"];
            } else if(ShortName == "bone_fragments") {
                GatherRate = (int) Config["bone_fragments"];
            }

            item.amount = item.amount * GatherRate;
        }
    }
}
