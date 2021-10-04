using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class TotalWire : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TotalWire");
            Tooltip.SetDefault("TotalWire");
            DisplayName.AddTranslation(GameCulture.Chinese, "总线");
            Tooltip.AddTranslation(GameCulture.Chinese, "制作计算机的材料之一");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 38;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(mod.GetItem("SuLiao"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
