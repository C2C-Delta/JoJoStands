﻿using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
    public class TheHandT3 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/TheHandT1"; }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nSpecial: Tap special to bring enemies to you and hold special to charge a scrape attack!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 52;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheHandT2"));
            recipe.AddIngredient(ItemID.TitaniumBar, 18);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheHandT2"));
            recipe.AddIngredient(ItemID.AdamantiteBar, 18);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
