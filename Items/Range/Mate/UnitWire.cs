using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Mate
{
    public class UnitWire : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("UnitWire");
            Tooltip.SetDefault("UnitWire");
            DisplayName.AddTranslation(GameCulture.Chinese, "集成电路模板");
            Tooltip.AddTranslation(GameCulture.Chinese, "制作CPU的材料之一");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 24;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.Glass, 20);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
