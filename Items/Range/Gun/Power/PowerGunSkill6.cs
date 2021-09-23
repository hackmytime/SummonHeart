using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Power;

namespace SummonHeart.Items.Range.Gun.Power
{
    public class PowerGunSkill6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PowerGunSkill6");
            Tooltip.SetDefault("PowerGunSkill6");
            DisplayName.AddTranslation(GameCulture.Chinese, "核心科技·枪械强化Lv6");
            Tooltip.AddTranslation(GameCulture.Chinese, "炼化压缩融合9把，伤害提升45倍，暴击几率+100%，" +
               "\n以1号物品栏内的武器为基准，左键使用消耗能量核心Lv1进行炼化压缩融合");
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
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技等级已满，无法升级");
            }
            else
            {
                Item baseItem = player.inventory[0];
                bool hasWeapon = true;
                int weaponCount = 8;
                ItemCost[] costArr = new ItemCost[] {
                    new ItemCost(ModContent.ItemType<Power6>(), 1),
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
                    bool flag = baseItem.ranged && baseItem.useAmmo == AmmoID.Bullet;
                    if (flag)
                    {
                        if (Builder.CanPayCost(costArr, player))
                        {
                            Builder.PayCost(costArr, player);
                            for (int i = 1; i <= weaponCount; i++)
                            {
                                Item item = player.inventory[i];
                                item.TurnToAir();
                            }
                            item.GetGlobalItem<SkillBase>().skillUseCount++;
                            baseItem.GetGlobalItem<SkillGItem>().skillType = SkillType.MultiGun;
                            baseItem.GetGlobalItem<SkillGItem>().skillLevel = 6;
                            baseItem.GetGlobalItem<SkillGItem>().curPower = 500000;
                            baseItem.GetGlobalItem<SkillGItem>().powerMax = 500000;
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
