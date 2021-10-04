using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class LightGenerator3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LightGenerator3");
            Tooltip.SetDefault("LightGenerator3");
            DisplayName.AddTranslation(GameCulture.Chinese, "高级闪电发射器");
            Tooltip.AddTranslation(GameCulture.Chinese, "高级闪电发射器");
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
            recipe.AddIngredient(mod.GetItem("LightGenerator2"), 5);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
