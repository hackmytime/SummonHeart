using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Power
{
    public class Power1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SoulCrystal");
            Tooltip.SetDefault("DemonLure, Consume 500 soul power and transfer it to the random treasure chest\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "能量核心Lv1");
            Tooltip.AddTranslation(GameCulture.Chinese, "能量上限1W，用完报废" +
                "\n最基础的能量核心，勉强能用");
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
