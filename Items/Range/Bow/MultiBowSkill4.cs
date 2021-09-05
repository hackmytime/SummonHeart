using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Power;

namespace SummonHeart.Items.Range.Bow
{
    public class MultiBowSkill4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MultiBowSkill4");
            Tooltip.SetDefault("MultiBowSkill4");
            DisplayName.AddTranslation(GameCulture.Chinese, "核心科技·弓弩组合科技Lv4");
            Tooltip.AddTranslation(GameCulture.Chinese, "前5号物品栏放置5把同类型的弓，左键使用消耗1个能量核心Lv4组合5把弓");
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
                mp.player.QuickSpawnItem(ModContent.ItemType<MultiBowSkill5>(), 1);
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技升级成功");
            }
            else
            {
                Item baseItem = player.inventory[0];
                bool hasWeapon = true;
                int weaponCount = 3;
                for (int i = 1; i <= weaponCount; i++)
                {
                    Item item = player.inventory[i];
                    if (item.type != baseItem.type)
                        hasWeapon = false;
                }
                ItemCost[] costArr = new ItemCost[] {
                new ItemCost(
                    ModContent.ItemType<Power4>(), 1)
                };
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (!hasWeapon)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "1、2、3、4、5号物品栏武器类型不同，无法合成");
                }
                else
                {
                    bool flag = baseItem.ranged && baseItem.useAmmo == AmmoID.Arrow;
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
                            baseItem.GetGlobalItem<SkillGItem>().skillType = SkillType.MultiBow;
                            baseItem.GetGlobalItem<SkillGItem>().skillLevel = 4;
                            baseItem.GetGlobalItem<SkillGItem>().curPower = 100000;
                            baseItem.GetGlobalItem<SkillGItem>().powerMax = 100000;
                        }
                    }
                    else
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "1、2、3、4、5号物品栏武器类型不是弓弩，无法合成");
                    }
                }
            }
            
            return true;
        }
      
    }
}
