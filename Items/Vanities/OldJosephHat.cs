﻿using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class OldJosephHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Man's Hat");
            Tooltip.SetDefault("A western hat. Comes with a beard!");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = ItemRarityID.LightPurple;
            item.vanity = true;
        }

        public override bool DrawHead()
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}