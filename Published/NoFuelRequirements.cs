namespace Oxide.Plugins
{
    [Info("No Fuel Requirements", "Waizujin", 1.1)]
    [Description("Allows you to choose which deployables do not use fuel.")]
    public class NoFuelRequirements : RustPlugin
    {
        protected override void LoadConfig()
        {
            bool dirty = false;
            base.LoadConfig();

            if (Config["campfire"] == null)
            {
                Config["campfire"] = false;
                dirty = true;
            }

            if (Config["lantern"] == null)
            {
                Config["lantern"] = false;
                dirty = true;
            }

            if (Config["furnace"] == null)
            {
                Config["furnace"] = false;
                dirty = true;
            }

            if (Config["minersHelmetAndCandleHat"] == null)
            {
                Config["minersHelmetAndCandleHat"] = false;
                dirty = true;
            }

            if (Config["quarry"] == null)
            {
                Config["quarry"] = false;
                dirty = true;
            }

            if (dirty)
            {
                PrintWarning("Updating configuration file with new values.");
                SaveConfig();
            }
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Creating a new configuration file.");
            Config.Clear();

            Config["campfire"] = false;
            Config["lantern"] = false;
            Config["furnace"] = false;
            Config["minersHelmetAndCandleHat"] = false;
            Config["quarry"] = false;

            SaveConfig();
        }

        void OnConsumeFuel(BaseOven oven, Item fuel, ItemModBurnable burnable)
        {
            /* Lantern */
            if ((bool) Config["lantern"])
            {
                if (oven.temperature == BaseOven.TemperatureType.Warming)
                {
                    ++fuel.amount;
                }
            }

            /* Campfire */
            if ((bool) Config["campfire"])
            {
                if (oven.temperature == BaseOven.TemperatureType.Cooking)
                {
                    ++fuel.amount;
                }
            }

            /* Furnace */
            if ((bool) Config["furnace"])
            {
                if (oven.temperature == BaseOven.TemperatureType.Smelting)
                {
                    ++fuel.amount;
                }
            }
        }

        void OnConsumableUse(Item item)
        {
            if ((bool) Config["minersHelmetAndCandleHat"])
            {
                if (item.parent.capacity == 1)
                {
                    ++item.amount;
                }
            }

            if ((bool) Config["quarry"])
            {
                if (item.parent.capacity == 18)
                {
                    ++item.amount;
                }
            }
        }
    }
}
