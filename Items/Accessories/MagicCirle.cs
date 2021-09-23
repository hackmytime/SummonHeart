using SummonHeart.Items.Scrolls;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Accessories
{
    public class MagicCirle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MagicCirle");
            Tooltip.SetDefault("MagicCirle");
            DisplayName.AddTranslation(GameCulture.Chinese, "拘禁套装");
            Tooltip.AddTranslation(GameCulture.Chinese, "对想自虐的人可能有用" +
                "\n玩家无敌帧降低到6帧");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(9, 0, 0, 0);
            item.rare = -12;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.immuneTime = 6;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Shackle, 4);
            modRecipe.AddIngredient(ItemID.ManaCrystal, 1);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
