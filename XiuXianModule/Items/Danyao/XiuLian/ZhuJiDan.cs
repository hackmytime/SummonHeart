using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using SummonHeart.XiuXianModule.Entities;
using SummonHeart.Buffs.XiuXian;

namespace SummonHeart.Items.Range.Loot
{
    public class ZhuJiDan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("筑基丹");
            Tooltip.SetDefault("三品丹药" +
                "\n可用于突破筑基" +
                "\n服用后灵力吸收速度增加8倍" +
                "\n持续2分钟");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 2;
            item.value = Item.sellPrice(1, 0, 0, 0);
            item.maxStack = 99;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            RPGPlayer mp = player.GetModPlayer<RPGPlayer>();
            if (mp.GetLevel() > 10)
            {
                CombatText.NewText(player.getRect(), Color.Red, "境界过高，此丹药对你已经无效，无法吸收");
                return false;
            }
            else
            {
                player.AddBuff(ModContent.BuffType<XisuiBuff>(), 3600 * 2);
            }
            return true;
        }
    }
}
