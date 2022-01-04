using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class LingShi3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("上品灵石");
            Tooltip.SetDefault("元婴境界的老怪物的流通灵石" +
                "\n蕴藏着10W灵力");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 5;
            item.value = Item.sellPrice(10, 0, 0, 0);
            item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi2"), 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi2"), 1000);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}
