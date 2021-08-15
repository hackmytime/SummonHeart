using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SummonHeart.Buffs.Weapon;
using SummonHeart.Extensions;
using SummonHeart.Projectiles.Weapon;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SummonHeart.Items.Weapons.Summon
{
    public class DemonFlySword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonFlySword");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔剑·神灭·传承武器");

            Tooltip.SetDefault(
              "Refining eight realms · the peak of martial arts · cast by the immortal right arm of the ancient demon God before he died" +
               "\nThe guardian weapon of the devil's son can only be summoned by blood essence" +
               "\nResentment of all living beings: critical hit bonus without any damage reduced by 4 times attack speed bonus and 2 times attack range bonus" +
               "\nPower of killing gods: kill any creature + 1 attack power, but limited by the upper limit of awakening." +
               "\nSword awakening: kill the strong, capture their flesh and soul, repair the sword body and increase the upper limit of awakening.");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "炼体八境·武道巅峰·远古魔神临死之前碎裂不朽右臂所铸造" +
                "\n魔神之子的护道传承武器，唯魔神之子可用精血召唤使用" +
                "\n众生之怨：1把神灭占用2召唤栏，最多召唤10把，召唤伤害恒为1，无法附魔，觉醒上限减半" +
                "\n弑神之力：击杀任意BOSS增加觉醒度，然受觉醒上限限制。" +
                "\n灵魂法则：自身蕴含魔神所悟灵魂法则之力，攻击造成附加真实伤害" +
                "\n魔剑觉醒：击杀强者摄其灵魂喂养剑灵，可突破觉醒上限。");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 1;
            item.useStyle = 1;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<DemonFlySwordMinion>();
            item.buffType = ModContent.BuffType<BladeStaffBuff>();
            item.width = 45;
            item.height = 45;
            item.UseSound = SoundID.Item113;
            item.useAnimation = 30;
            item.useTime = 30;
            item.noMelee = true;
            item.knockBack = 0f;
            item.rare = -12;
            item.value = Item.sellPrice(999, 0, 0, 0);
            item.summon = true;
            item.autoReuse = true;
        }

        public override bool AllowPrefix(int pre)
        {
            return false;
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return new bool?(false);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (modPlayer.flySwordBlood == 0)
                modPlayer.flySwordBlood = 1;
            if (modPlayer.swordBloodMax < 100)
                modPlayer.swordBloodMax = 100;

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("Damage"));
            if (num != -1)
            {
                string str = (modPlayer.flySwordBlood * 1.0f / 100f).ToString("0.00") + "%";
                string text;
                text = modPlayer.flySwordBlood + " 真实伤害";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightSkyBlue;
                tooltips.Insert(num + 1, tooltipLine);
                text = str + (GameCulture.Chinese.IsActive ? "觉醒度" : "Arousal Level");
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LimeGreen;
                tooltips.Insert(num + 2, tooltipLine);
                text = "击杀Boss+" + (modPlayer.swordBloodMax / 2000 + 5) + "真实伤害";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 3, tooltipLine);
                text = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00") + "%觉醒上限";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 4, tooltipLine);

                text = player.getDownedBoss();
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightGreen;
                tooltips.Insert(tooltips.Count, tooltipLine);
            }
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            damage = 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2, true);
            position = Main.MouseWorld;
            if (player.ownedProjectileCounts(mod.ProjectileType("DemonFlySwordMinion")) < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.AddIngredient(mod.GetItem("SummonScroll"), 1);
            recipe.AddIngredient(ItemID.LifeCrystal, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
