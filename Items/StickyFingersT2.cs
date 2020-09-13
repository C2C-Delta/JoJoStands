using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersT2 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 37;
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
            recipe.AddIngredient(mod.ItemType("StickyFingersT1"));
            recipe.AddIngredient(ItemID.Sapphire, 3);
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
