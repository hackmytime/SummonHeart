using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Accessories
{
    public class MysteriousCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MysteriousCrystal");
            Tooltip.SetDefault("Resurrection time reduced to 5 seconds");
            DisplayName.AddTranslation(GameCulture.Chinese, "神秘水晶");
            Tooltip.AddTranslation(GameCulture.Chinese, "复活时间减为5秒\n给挑战者的礼物");
        }

        public override void SetDefaults()
        {
            item.rare = 4;
            item.width = 32;
            item.height = 32;
            item.value = 4 * 100000;
            item.accessory = true;
        }
    }
}