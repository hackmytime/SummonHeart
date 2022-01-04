using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.XiuXianModule.Items.LingShi
{
    public class LingShi4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("极品灵石");
            Tooltip.SetDefault("化神及以上境界的老怪物才能拿出来的下界最强灵石" +
                "\n蕴藏着1000W灵力");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 7;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi3"), 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi3"), 1000);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}
