using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class NeiDan3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("NeiDan3");
            Tooltip.SetDefault("NeiDan3");
            DisplayName.AddTranslation(GameCulture.Chinese, "三阶内丹");
            Tooltip.AddTranslation(GameCulture.Chinese, "结丹期灵兽一生所修之精元内丹" +
                "\n蕴藏着庞大无比的能量");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 3;
            item.value = Item.sellPrice(0, 100, 0, 0);
            item.maxStack = 9999;
        }
    }
}
