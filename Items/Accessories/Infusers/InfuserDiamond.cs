﻿﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DBZMOD.Items.Accessories.Infusers
{
    public class InfuserDiamond : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies with ki attacks inflicts confusion.");
            DisplayName.SetDefault("Diamond Ki Infuser");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 30;
            item.value = 3200;
            item.rare = 4;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            {
                player.GetModPlayer<MyPlayer>(mod).infuserDiamond = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StableKiCrystal", 25);
            recipe.AddIngredient(null, "ScrapMetal", 12);
            recipe.AddIngredient(ItemID.Diamond, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}