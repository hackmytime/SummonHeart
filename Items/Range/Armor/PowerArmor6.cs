using SummonHeart.Items.Scrolls;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Armor
{
    public class PowerArmor6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PowerArmor6");
            Tooltip.SetDefault("PowerArmor6");
            DisplayName.AddTranslation(GameCulture.Chinese, "科技造物·能量护甲Lv6");
            Tooltip.AddTranslation(GameCulture.Chinese, "能量护盾上限500W" +
                "\n减伤500W后护甲报废");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.rare = -12;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PowerArmorBase powerArmorBase = item.GetGlobalItem<PowerArmorBase>();
            if (powerArmorBase.powerArmorMax == 0)
            {
                //初始化
                powerArmorBase.powerArmorMax = 5000000;
                powerArmorBase.powerArmorCount = 5000000;
            }
        }
    }
}
