using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.AmmoSkill
{
    public class VUnit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("VUnit");
            Tooltip.SetDefault("VUnit");
            DisplayName.AddTranslation(GameCulture.Chinese, "穿透元件");
            Tooltip.AddTranslation(GameCulture.Chinese, "蕴含大地之力的炼金元件" +
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
            recipe.AddIngredient(mod.GetItem("WaterDirt"), 1);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("WaterStone"), 1);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
