using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.XiuXianModule.Items.NeiDan
{
    public class NeiDan2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("NeiDan2");
            Tooltip.SetDefault("NeiDan2");
            DisplayName.AddTranslation(GameCulture.Chinese, "二阶内丹");
            Tooltip.AddTranslation(GameCulture.Chinese, "筑基期灵兽一生所修之精元内丹" +
                "\n蕴藏着庞大无比的能量");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 2;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.maxStack = 9999;
        }
    }
}
