using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.XiuXianModule.Items.LingShi
{
    public class LingShi1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("下品灵石");
            Tooltip.SetDefault("普通练气筑基修士的流通灵石" +
                "\n蕴藏着10灵力");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.maxStack = 9999;
        }
    }
}
