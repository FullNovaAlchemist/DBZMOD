﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DBZMOD.Items
{
    public class DemonicSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonic Soul");
            Tooltip.SetDefault("'A fragment of the devil's rage.'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.maxStack = 9999;
            item.value = 800;
            item.rare = 7;
        }
	    public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}