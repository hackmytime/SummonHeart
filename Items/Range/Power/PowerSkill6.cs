using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;

namespace SummonHeart.Items.Range.Power
{
    public class PowerSkill6 : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PowerSkill6");
            Tooltip.SetDefault("PowerSkill6");
            DisplayName.AddTranslation(GameCulture.Chinese, "核心科技·动力能源科技Lv6");
            Tooltip.AddTranslation(GameCulture.Chinese, "从灵魂之力中提取能源的科技" +
                "\n左键使用炼金术消耗5W灵魂之力制作能量核心Lv6，能量上限50W");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.useAnimation = 10;
            item.useTime = 10;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }

        public override void UpdateInventory(Player player)
        {
            item.GetGlobalItem<SkillBase>().levelUpCount = 20;
        }

        public override bool AltFunctionUse(Player player)
        {
            SkillBase skillBase = item.GetGlobalItem<SkillBase>();
            if (skillBase.skillUseCount >= skillBase.levelUpCount)
            {
                return true;
            }
            return base.AltFunctionUse(player);
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            int costSoul = 50000;
            if (player.altFunctionUse == 2)
            {
                //处理升级
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技等级已满，无法升级");
            }
            else
            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (mp.CheckSoul(costSoul))
                {
                    mp.BuySoul(costSoul);
                    mp.player.QuickSpawnItem(ModContent.ItemType<Power6>(), 1);
                    item.GetGlobalItem<SkillBase>().skillUseCount++;
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足");

                }
            }
            return true;
        }
    }
}
