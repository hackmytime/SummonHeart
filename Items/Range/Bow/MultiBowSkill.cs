using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Range;

namespace SummonHeart.Items.Range.Bow
{
    public class MultiBowSkill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SoulCrystal");
            Tooltip.SetDefault("DemonLure, Consume 500 soul power and transfer it to the random treasure chest\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "弓弩组合科技Lv1");
            Tooltip.AddTranslation(GameCulture.Chinese, "使用能量核心作为动力源，利用机械架构组合5把同类型的弓，使得同时使用5把弓。每一次攻击消耗5能量。");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }

        public override void RightClick(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.BBP < 1)
            {
                player.statLife = 1;
                CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足，强行使用生命值减为1");
            }
            else
            {
                bool flag = player.HeldItem.ranged;
                if (flag)
                {
                    player.HeldItem.GetGlobalItem<SkillGItem>().skillType = SkillType.MultiBow;
                    player.HeldItem.GetGlobalItem<SkillGItem>().skillLevel = 1;
                }
            }
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            Item baseItem = player.inventory[0];
            if(mp.PlayerClass != 7)
            {
                CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
            }
            else if (baseItem == null)
            {
                CombatText.NewText(player.getRect(), Color.Red, "1号物品栏无武器");
            }
            else
            {
                bool flag = baseItem.ranged;
                if (flag)
                {
                    baseItem.GetGlobalItem<SkillGItem>().skillType = SkillType.MultiBow;
                    baseItem.GetGlobalItem<SkillGItem>().skillLevel = 1;
                }
            }
            return true;
        }

    }
}
