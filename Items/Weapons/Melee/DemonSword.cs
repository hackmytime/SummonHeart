using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Projectiles.Melee;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SummonHeart.Items.Weapons.Melee
{
    internal class DemonSword : ModItem
    {
        private const int dustId = MyDustId.BlueMagic;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonSword");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔剑·弑神·传承武器");

            Tooltip.SetDefault(
               "Refining eight realms · the peak of martial arts · cast by the immortal right arm of the ancient demon God before he died" +
                "\nThe guardian weapon of the devil's son can only be summoned by blood essence" +
                "\nResentment of all living beings: critical hit bonus without any damage reduced by 4 times attack speed bonus and 2 times attack range bonus" +
                "\nPower of killing gods: kill any creature + 1 attack power, but limited by the upper limit of awakening." +
                "\nSword awakening: kill the strong, capture their flesh and soul, repair the sword body and increase the upper limit of awakening.");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "炼体八境·武道巅峰·远古魔神临死之前碎裂不朽右臂所铸造" +
                "\n魔神之子的护道传承武器，唯魔神之子可用精血召唤使用" +
                "\n空间法则：攻击范围内攻击无视距离" +
                "\n魔剑觉醒：击杀强者摄其血肉灵魂修复剑身可增加基础伤害。");
        }

        public override void SetDefaults()
        {
            item.useStyle = 3;
            item.shootSpeed = 1f;
            item.shoot = ModContent.ProjectileType<DragonLegacyRed>();
            item.noUseGraphic = true;
            item.melee = true;
            item.noMelee = true;
            item.damage = getDownedBossDmage();
            item.knockBack = 3;
            item.autoReuse = true;
            //item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 22;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            position = Main.MouseWorld;
            //计算距离
            float dist = Vector2.Distance(position, player.Center);
            float attackRange = 500f;
            if (dist > attackRange)
            {
                Vector2 basePos = Vector2.Normalize(Main.MouseWorld - player.Center);
                position = basePos * attackRange + player.Center;
            }
            position += Main.rand.NextVector2(20f, 22f);

            NPC target;
            if ((target = Helper.GetNearestNPC(position, (NPC npc) => npc.active && !npc.friendly && !npc.dontTakeDamage, attackRange)) != null)
            {
                position = target.Center;
            }
            Vector2 vel = VectorHelper.VelocityToPoint(player.Center, position, 1f);
            float num;
            float num2;
            Utils.RotatedByRandom(VectorHelper.VelocityToPoint(player.Center, position, 1f), 0.6).Deconstruct(out num, out num2);
            speedX = num;
            speedY = num2;
            position -= vel * 120f;
            return true;
        }

        public override bool AllowPrefix(int pre)
        {
            return false;
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return new bool?(false);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.AddIngredient(mod.GetItem("MeleeScroll"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
            if (num != -1)
            {
                string text;
                float attackRange = 500f;
                text = "攻击范围 " + attackRange + "格";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightGreen;
                tooltips.Insert(num + 1, tooltipLine);

                Player player = Main.player[Main.myPlayer];
                text = player.getDownedBoss();
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightGreen;
                tooltips.Insert(tooltips.Count, tooltipLine);
            }
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if(mp.PlayerClass == 1)
            {
                crit = 100;
            }
            else if(mp.PlayerClass == 4)
            {
                crit = mp.angerResourceCurrent;
            }
        }

        public int getDownedBossDmage()
        {
            int[] bossTips = new int[]
            {
                25,
                26,
                28,
                30,
                35,
                42,
                56,
                70,
                90,
                120,
                140,
                160,
                200
            };
            int downedIndex = 0;
            //1、2W - king slime5 %
            if (NPC.downedSlimeKing)
            {
                downedIndex = 1;
            }
            //2、3W - bigEye10
            if (NPC.downedBoss1)
            {
                downedIndex = 2;
            }
            //3、4W - 世吞 / 克脑20
            if (NPC.downedBoss2)
            {
                downedIndex = 3;
            }
            //4、6W - 蜂王30
            if (NPC.downedQueenBee)
            {
                downedIndex = 4;
            }
            //5、7W - 吴克40
            if (NPC.downedBoss3)
            {
                downedIndex = 5;
            }
            //6、8W - 肉山50
            if (Main.hardMode)
            {
                downedIndex = 6;
            }
            //7、10W-新三王80
            if (NPC.downedMechBossAny)
            {
                downedIndex = 7;
            }
            //8、12W - 小花100
            if (NPC.downedPlantBoss)
            {
                downedIndex = 8;
            }
            //9、14W - 小怪120
            if (NPC.downedFishron)
            {
                downedIndex = 9;
            }
            //10、16W - 石头150
            if (NPC.downedGolemBoss)
            {
                downedIndex = 10;
            }
            //11、18W - 教徒200
            if (NPC.downedAncientCultist)
            {
                downedIndex = 11;
            }

            //12、20W - 月总无上限*/
            if (NPC.downedMoonlord)
            {
                downedIndex = 12;
            }
            return bossTips[downedIndex];
        }
    }
}
