using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Loot
{
    public class Loot4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot4");
            Tooltip.SetDefault("Loot4");
            DisplayName.AddTranslation(GameCulture.Chinese, "4级生物材料");
            Tooltip.AddTranslation(GameCulture.Chinese, "用炼金术炼化敌人身躯形成的固态精华" +
                "\n蕴含巨量生物之精华");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 8;
            item.value = Item.sellPrice(10, 0, 0, 0);
            item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot3"), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
