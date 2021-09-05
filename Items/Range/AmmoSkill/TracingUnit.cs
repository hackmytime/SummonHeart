using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.AmmoSkill
{
    public class TracingUnit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TracingUnit");
            Tooltip.SetDefault("TracingUnit");
            DisplayName.AddTranslation(GameCulture.Chinese, "追踪元件");
            Tooltip.AddTranslation(GameCulture.Chinese, "蕴含灵魂力量的炼金元件" +
                "\n造价高昂，品质保证");
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
