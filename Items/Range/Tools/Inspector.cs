using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools
{
    public class Inspector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("武器强化台");
            Tooltip.SetDefault("放置后右键打开强化UI");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 1;
            item.value = Item.buyPrice(0, 3, 80, 0);
            item.createTile = mod.TileType("InspectorTile");
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddRecipeGroup(RecipeGroupID.IronBar, 50);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
