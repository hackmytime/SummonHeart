using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Loot
{
    public class Loot1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot1");
            Tooltip.SetDefault("Loot1");
            DisplayName.AddTranslation(GameCulture.Chinese, "1级生物材料");
            Tooltip.AddTranslation(GameCulture.Chinese, "用炼金术炼化敌人身躯形成的固态精华" +
                "\n蕴含各种实体物质");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.maxStack = 9999;
        }
    }
}
