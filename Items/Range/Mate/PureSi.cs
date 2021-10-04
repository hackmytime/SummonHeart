using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class PureSi : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PureSi");
            Tooltip.SetDefault("PureSi");
            DisplayName.AddTranslation(GameCulture.Chinese, "纯硅");
            Tooltip.AddTranslation(GameCulture.Chinese, "制作芯片的原材料");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 40;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SandBlock, 100);
            recipe.AddIngredient(ItemID.AshBlock, 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
