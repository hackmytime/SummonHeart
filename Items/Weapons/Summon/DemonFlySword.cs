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
                "\n众生之怨：1把神灭占用2召唤栏，最多召唤10把" +
                "\n魔剑觉醒：击杀强者摄其灵魂喂养剑灵，可增加基础攻击");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = getDownedBossDmage();
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

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            string text = "";
            text = player.getDownedBoss();
            TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
            tooltipLine.overrideColor = Color.LightGreen;
            tooltips.Insert(tooltips.Count, tooltipLine);
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
