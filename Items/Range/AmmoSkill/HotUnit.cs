using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.AmmoSkill
{
    public class HotUnit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HotUnit");
            Tooltip.SetDefault("HotUnit");
            DisplayName.AddTranslation(GameCulture.Chinese, "加热元件");
            Tooltip.AddTranslation(GameCulture.Chinese, "蕴含火焰之力的炼金元件" +
                "\n造价高昂，品质保证");
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
            recipe.AddIngredient(ItemID.Hellstone, 20);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
