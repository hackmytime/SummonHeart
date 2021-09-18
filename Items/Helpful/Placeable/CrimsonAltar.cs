using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Helpful.Placeable
{
    public class CrimsonAltar : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 20;
            item.maxStack = 1;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 1;
            item.placeStyle = 1;
            item.createTile = 26;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Altar");
            Tooltip.SetDefault("It is a Demon Altar. Careful where you place!");
            DisplayName.AddTranslation(GameCulture.Chinese, "血腥祭坛");
            Tooltip.AddTranslation(GameCulture.Chinese, "这是一个血腥祭坛.小心地放下来!");
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(836, 30);
            modRecipe.AddIngredient(1330, 25);
            modRecipe.AddTile(16);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
