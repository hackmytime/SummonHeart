using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class NeiDan7 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("七阶内丹");
            Tooltip.SetDefault("渡劫期灵兽一生所修之精元内丹" +
                "\n蕴藏着庞大无比的能量");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 7;
            item.value = Item.sellPrice(1000, 0, 0, 0);
            item.maxStack = 9999;
        }
    }
}
