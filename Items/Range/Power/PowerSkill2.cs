using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Material;
using SummonHeart.Items.Range.Armor;

namespace SummonHeart.Items.Range.Power
{
    public class PowerSkill2 : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SoulCrystal");
            Tooltip.SetDefault("DemonLure, Consume 500 soul power and transfer it to the random treasure chest\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "核心科技·动力能源科技Lv2");
            Tooltip.AddTranslation(GameCulture.Chinese, "从可燃物木头、凝胶之中提取能源的科技" +
                "\n左键使用炼金术消耗500木头或者500凝胶制作能量核心Lv2，能量上限3W，用完报废。");
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
            ItemCost[] costArr1 = new ItemCost[] {
                new ItemCost(ItemID.Wood, 500)
            };
            ItemCost[] costArr2 = new ItemCost[] {
                new ItemCost(ItemID.Gel, 500)
            };
            if (player.altFunctionUse == 2)
            {
                //处理升级
                mp.player.QuickSpawnItem(ModContent.ItemType<PowerArmorSkill3>(), 1);
                mp.player.QuickSpawnItem(ModContent.ItemType<DemonBuff>(), 1);
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技升级成功");
                item.TurnToAir();
            }
            else
            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (Builder.CanPayCost(costArr1, player))
                {
                    Builder.PayCost(costArr1, player);
                    mp.player.QuickSpawnItem(ModContent.ItemType<Power2>(), 1);
                    item.GetGlobalItem<SkillBase>().skillUseCount++;
                }
                else if (Builder.CanPayCost(costArr2, player))
                {
                    Builder.PayCost(costArr2, player);
                    mp.player.QuickSpawnItem(ModContent.ItemType<Power2>(), 1);
                    item.GetGlobalItem<SkillBase>().skillUseCount++;
                }
            }
            return true;
        }
    }
}
