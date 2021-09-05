using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Power
{
    public class Power4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power4");
            Tooltip.SetDefault("Power4\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "能量核心Lv4");
            Tooltip.AddTranslation(GameCulture.Chinese, "能量上限10W，可以消耗能量核心进行充能" +
                "\n基准能量核心，可充能的制式装备");
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
