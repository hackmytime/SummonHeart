using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.Tile
{
    public class WaterStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WaterDirt");
            Tooltip.SetDefault("WaterDirt");
            DisplayName.AddTranslation(GameCulture.Chinese, "液态石");
            Tooltip.AddTranslation(GameCulture.Chinese, "石之精华");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 9999;
        }
    }
}
