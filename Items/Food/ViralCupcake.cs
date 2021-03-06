using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralCupcake : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A well-made cupcake on a tiny plate, all with a tiny viral structure on top!\nGrants Stand related buffs upon consumption.");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(0, 0, 12, 50);
            item.UseSound = SoundID.Item2;
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(mod.BuffType("StrongWill"), (2 * 60) * 60);
            player.AddBuff(mod.BuffType("SharpMind"), (2 * 60) * 60);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralPowder"), 4);
            recipe.AddIngredient(ItemID.HoneyBlock);
            recipe.AddIngredient(ItemID.Hay);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
