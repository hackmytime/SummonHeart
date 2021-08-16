using SummonHeart.Items.Scrolls;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Accessories
{
    public class MagicCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MagicBook");
            Tooltip.SetDefault("Resurrection time reduced to 5 seconds");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔力之源·传承饰品");
            Tooltip.AddTranslation(GameCulture.Chinese, "控法者职业传承饰品" +
                "\n魔法上限翻倍" +
                "\n魔法回复速度翻倍" +
                "\n可以使用快捷键自动充能，默认V请设置控件" +
                "\n仅对控法者职业有效");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.rare = -12;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SummonHeartPlayer>().magicBook = true;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<GuideNote>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<MagicScroll2>(), 1);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
