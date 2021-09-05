using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Power
{
    public class Power6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power6");
            Tooltip.SetDefault("Power6\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "能量核心Lv6");
            Tooltip.AddTranslation(GameCulture.Chinese, "能量上限50W，可以消耗能量核心进行充能" +
                "\n能量科技的巅峰之作");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
        }
    }
}
