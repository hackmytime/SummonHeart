using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Power;

namespace SummonHeart.Items.Range.Bow.Power
{
    public class PowerBowSkill4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PowerBowSkill4");
            Tooltip.SetDefault("PowerBowSkill4");
            DisplayName.AddTranslation(GameCulture.Chinese, "核心科技·弓弩强化Lv4");
            Tooltip.AddTranslation(GameCulture.Chinese, "炼化压缩融合7把，蓄力上限800，蓄力速度，1帧4层" +
                "\n每1层蓄力伤害提升0.2倍，暴击几率+1%，每1层蓄力消耗4点能量" +
                 "\n以1号物品栏的弓弩为基准，左键使用消耗能量核心Lv1进行炼化压缩融合");
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
            item.GetGlobalItem<SkillBase>().levelUpCount = 10;
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
            if (player.altFunctionUse == 2)
            {
                //处理升级
                item.TurnToAir();
                mp.player.QuickSpawnItem(ModContent.ItemType<PowerBowSkill5>(), 1);
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技升级成功");
            }
            else
            {
                Item baseItem = player.inventory[0];
                bool hasWeapon = true;
                int weaponCount = 6;
                ItemCost[] costArr = new ItemCost[] {
                    new ItemCost(ModContent.ItemType<Power4>(), 1),
                    new ItemCost(baseItem.type, weaponCount)
                };
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (!hasWeapon)
                {
                    CombatText.NewText(player.getRect(), Color.Red, $"前{weaponCount}号物品栏武器类型不同，无法合成");
                }
                else
                {
                    bool flag = baseItem.ranged && baseItem.useAmmo == AmmoID.Arrow;
                    if (flag)
                    {
                        if (Builder.CanPayCost(costArr, player))
                        {
                            Builder.PayCost(costArr, player);
                            item.GetGlobalItem<SkillBase>().skillUseCount++;
                            baseItem.GetGlobalItem<SkillGItem>().skillType = SkillType.PowerBow;
                            baseItem.GetGlobalItem<SkillGItem>().skillLevel = 4;
                            baseItem.GetGlobalItem<SkillGItem>().curPower = 100000;
                            baseItem.GetGlobalItem<SkillGItem>().powerMax = 100000;
                        }
                    }
                    else
                    {
                        CombatText.NewText(player.getRect(), Color.Red, $"前{weaponCount}号物品栏武器类型不同，无法合成");
                    }
                }
            }

            return true;
        }

    }
}
