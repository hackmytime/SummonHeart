using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.AmmoSkill;
using SummonHeart.Items.Range.Power;

namespace SummonHeart.Items.Range.Armor
{
    public class PowerArmorSkill5 : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PowerSkill5");
            Tooltip.SetDefault("PowerSkill5");
            DisplayName.AddTranslation(GameCulture.Chinese, "核心科技·能量护甲制造Lv5");
            Tooltip.AddTranslation(GameCulture.Chinese, "通过炼金术将金属元件和能量核心炼化成能量护甲" +
                "\n左键使用炼金术消耗500金属元件和10个能量核心Lv5进行制造");
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
            ItemCost[] costArr = new ItemCost[] {
                new ItemCost(ModContent.ItemType<MetalUnit>(), 500),
                new ItemCost(ModContent.ItemType<Power5>(), 10)
            };
            if (player.altFunctionUse == 2)
            {
                //处理升级
                mp.player.QuickSpawnItem(ModContent.ItemType<PowerArmorSkill6>(), 1);
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技升级成功");
                item.TurnToAir();
            }
            else
            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (Builder.CanPayCost(costArr, player))
                {
                    Builder.PayCost(costArr, player);
                    mp.player.QuickSpawnItem(ModContent.ItemType<PowerArmor5>(), 1);
                    item.GetGlobalItem<SkillBase>().skillUseCount++;
                }
            }
            return true;
        }
    }
}
