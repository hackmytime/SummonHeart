using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class SuLiao : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SuLiao");
            Tooltip.SetDefault("SuLiao");
            DisplayName.AddTranslation(GameCulture.Chinese, "高分子聚合物");
            Tooltip.AddTranslation(GameCulture.Chinese, "就是塑料");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 38;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot1"), 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
