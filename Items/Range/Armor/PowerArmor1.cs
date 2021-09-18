using SummonHeart.Items.Scrolls;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Armor
{
    public class PowerArmor1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PowerArmor1");
            Tooltip.SetDefault("PowerArmor1");
            DisplayName.AddTranslation(GameCulture.Chinese, "科技造物·能量护甲Lv1");
            Tooltip.AddTranslation(GameCulture.Chinese, "减伤倍率2，能量护盾上限10W" +
                "\n减伤20W后护甲报废");
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
                powerArmorBase.powerArmorMax = 200000;
                powerArmorBase.powerArmorCount = 200000;
            }
            //player.GetModPlayer<SummonHeartPlayer>().powerArmor = true;
        }
    }
}
