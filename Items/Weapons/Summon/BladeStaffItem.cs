﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SummonHeart.Buffs.Weapon;
using SummonHeart.Projectiles.Weapon;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SummonHeart.Items.Weapons.Summon
{
    public class BladeStaffItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade Staff");
            //Tooltip.SetDefault("Summons an Enchanted Dagger to fight for you\nIgnores a substantial amount of enemy defense\n'Don't let their small size fool you'");
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
                "\n众生之怨：不受任何伤害暴击攻击范围加成，无法附魔，减少2倍攻速加成" +
                "\n弑神之力：击杀任意BOSS增加攻击力，然受觉醒上限限制。" +
                "\n魔剑觉醒：击杀强者摄其血肉灵魂修复剑身，可突破觉醒上限。" +
                "\n灵魂法则：自身蕴含魔神所悟灵魂法则之力，攻击造成真实伤害");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 1;
            item.useStyle = 1;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<BladeStaffMinion>();
            item.buffType = ModContent.BuffType<BladeStaffBuff>();
            item.width = 45;
            item.height = 45;
            item.UseSound = SoundID.Item113;
            item.useAnimation = 30;
            item.useTime = 30;
            item.rare = 5;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(99, 0, 0, 0);
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

            if (modPlayer.swordBlood == 0)
                modPlayer.swordBlood = 1;
            if (modPlayer.swordBloodMax < 100)
                modPlayer.swordBloodMax = 100;

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
            if (num != -1)
            {
                string str = (modPlayer.swordBlood * 1.0f / 100f).ToString("0.00") + "%";
                tooltips[num].overrideColor = Color.LimeGreen;
                tooltips[num].text = str + (GameCulture.Chinese.IsActive ? "觉醒度" : "Arousal Level");
                string text;
                text = "击杀敌人+" + (modPlayer.swordBloodMax / 10000 + 1) + "攻击力";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 1, tooltipLine);
                text = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00") + "%觉醒上限";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 2, tooltipLine);
            }
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            damage = modPlayer.swordBlood;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2, true);
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.AddIngredient(mod.GetItem("SummonScroll"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
