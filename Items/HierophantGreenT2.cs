using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class HierophantGreenT2 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hierophant Green (Tier 2)");
			Tooltip.SetDefault("Shoot emeralds at the enemies!\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 32;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HierophantGreenT1"));
            recipe.AddIngredient(ItemID.Emerald, 2);
            recipe.AddIngredient(ItemID.Hellstone, 10);
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
