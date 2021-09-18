using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Helpful.Placeable
{
    public class DemonAltar : ModItem
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
            item.createTile = 26;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Altar");
            Tooltip.SetDefault("It is a Demon Altar. Careful where you place!");
            DisplayName.AddTranslation(GameCulture.Chinese, "恶魔祭坛");
            Tooltip.AddTranslation(GameCulture.Chinese, "这是一个恶魔祭坛.小心地放下来!");
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(61, 30);
            modRecipe.AddIngredient(68, 10);
            modRecipe.AddIngredient(69, 5);
            modRecipe.AddTile(16);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
