using SummonHeart.Items.Scrolls;
using System;
using Terraria;
using Terraria.ID;
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
            DisplayName.AddTranslation(GameCulture.Chinese, "魔力之源·传奇饰品");
            Tooltip.AddTranslation(GameCulture.Chinese, "传说中的物品" +
                "\n魔法回复速度翻倍");
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
            player.GetModPlayer<SummonHeartPlayer>().magicBook = true;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.ManaFlower, 1);
            modRecipe.AddIngredient(ItemID.RazorbladeTyphoon, 1);
            modRecipe.AddIngredient(ItemID.ManaCrystal, 999);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
