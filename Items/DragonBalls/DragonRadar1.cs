﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.DragonBalls
{
    public class DragonRadar1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon Radar MK1");
            Tooltip.SetDefault("A low tech piece of equipment used to locate dragon balls." +
                "\nHolding this will point you in the direction of the nearest dragon ball with bad accuracy," +
                "\nGetting too close to a dragon ball will overload the radar.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 0;
            item.rare = -12;
        }
    }
}