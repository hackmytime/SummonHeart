using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Power
{
    public class Power6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Power6");
            Tooltip.SetDefault("Power6\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "能量核心Lv6");
            Tooltip.AddTranslation(GameCulture.Chinese, "能量上限50W，左键可以消耗能量核心对手持6级科技武器进行充能" +
                "\n能量科技的巅峰之作");
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            Item baseItem = player.HeldItem;
            if(baseItem != null)
            {
                SkillGItem skillGItem = baseItem.GetGlobalItem<SkillGItem>();
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (skillGItem.skillLevel != 6)
                {
                    CombatText.NewText(player.getRect(), Color.Red, $"当前武器非6级科技造物，无法充能");
                }
                else if (skillGItem.curPower > 10000)
                {
                    CombatText.NewText(player.getRect(), Color.Red, $"当前武器还有1W以上能量，浪费可耻");
                }
                else
                {
                    skillGItem.curPower = 500000;
                    CombatText.NewText(player.getRect(), Color.LightGreen, $"充能完成");
                    item.TurnToAir();
                }
            }

            return true;
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 99;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }
    }
}
