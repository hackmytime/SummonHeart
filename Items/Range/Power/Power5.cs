using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Power
{
    public class Power5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power5");
            Tooltip.SetDefault("Power5\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "能量核心Lv5");
            Tooltip.AddTranslation(GameCulture.Chinese, "能量上限20W，用完报废" +
                "\n强力能量核心，核心动力源");
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
