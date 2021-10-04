using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class CoreComputer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CoreComputer");
            Tooltip.SetDefault("CoreComputer");
            DisplayName.AddTranslation(GameCulture.Chinese, "微型计算机");
            Tooltip.AddTranslation(GameCulture.Chinese, "高级科技产物，拥有接收并处理信息的能力");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("CPU"), 1);
            recipe.AddIngredient(mod.GetItem("ZhuBan"), 1);
            recipe.AddIngredient(mod.GetItem("TotalWire"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
