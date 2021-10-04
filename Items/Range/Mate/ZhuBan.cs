using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class ZhuBan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ZhuBan");
            Tooltip.SetDefault("ZhuBan");
            DisplayName.AddTranslation(GameCulture.Chinese, "主板");
            Tooltip.AddTranslation(GameCulture.Chinese, "制作计算机的材料之一");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 36;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 5);
            recipe.AddIngredient(mod.GetItem("PureSi"), 1);
            recipe.AddIngredient(mod.GetItem("SuLiao"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
