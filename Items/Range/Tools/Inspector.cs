using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools
{
    public class Inspector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lab Terminal");
            Tooltip.SetDefault("Right click to open up UI - Technical Fabricator.\n\t Scans samples for DNA.\n\t Catalogs complete genomes.\n\t Fabricates an index to serve as modem for biological organisms.");
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
            modRecipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            modRecipe.AddIngredient(178, 1);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
