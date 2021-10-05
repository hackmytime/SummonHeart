using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class LightGenerator1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LightGenerator1");
            Tooltip.SetDefault("LightGenerator1");
            DisplayName.AddTranslation(GameCulture.Chinese, "初级闪电发射器");
            Tooltip.AddTranslation(GameCulture.Chinese, "初级闪电发射器");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 42;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 500);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 50);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
