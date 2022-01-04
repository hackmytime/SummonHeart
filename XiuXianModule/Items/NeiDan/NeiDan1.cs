using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class NeiDan1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("一阶内丹");
            Tooltip.SetDefault("炼气期灵兽一生所修之精元内丹" +
                "\n蕴藏着庞大无比的能量");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.maxStack = 9999;
        }
    }
}
