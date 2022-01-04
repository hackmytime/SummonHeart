using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.XiuXianModule.Items.LingShi
{
    public class LingShi2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("中品灵石");
            Tooltip.SetDefault("结丹修士高手的流通灵石" +
                "\n蕴藏着1000灵力");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 3;
            item.value = Item.sellPrice(0, 100, 0, 0);
            item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi1"), 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi1"), 1000);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}
