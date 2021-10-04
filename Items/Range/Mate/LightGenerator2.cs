using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class LightGenerator2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LightGenerator2");
            Tooltip.SetDefault("LightGenerator2");
            DisplayName.AddTranslation(GameCulture.Chinese, "中级闪电发射器");
            Tooltip.AddTranslation(GameCulture.Chinese, "中级闪电发射器");
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
            recipe.AddIngredient(mod.GetItem("LightGenerator1"), 5);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
