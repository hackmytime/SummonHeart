using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SummonHeart.Items.Weapons.Magic
{
    // Token: 0x02000311 RID: 785
    public class DemonStaff : ModItem
    {
        // Token: 0x0600135A RID: 4954 RVA: 0x000B4DC0 File Offset: 0x000B2FC0
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Delicious Saturn");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔剑·万剑归宗");
            Tooltip.SetDefault("Creates a whirlwind of blades upon hit\nEach blade increases your defense");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "炼体八境·武道巅峰·远古魔神临死之前碎裂不朽右臂所铸造" +
                "\n魔神之子的护道传承武器，唯魔神之子可用精血召唤使用" +
                "\n破灭法则：暴击几率翻倍，穿透+6" +
                "\n魔源觉醒：击杀强者摄其血肉灵魂可增加基础攻击。");
        }

        // Token: 0x0600135B RID: 4955 RVA: 0x000B4E18 File Offset: 0x000B3018
        public override void SetDefaults()
        {
            item.damage = getDownedBossDmage();
            item.width = 46;
            item.height = 46;
            /* item.useTime = 20;
            item.useAnimation = 20;*/
            item.knockBack = 3.0f;
            item.rare = -12;
            item.value = Item.sellPrice(999, 0, 0, 0);
            item.autoReuse = true;
            item.magic = true;
            item.mana = 10;
            item.noMelee = true;
            Item.staff[item.type] = true;
            item.UseSound = SoundID.Item20;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            //item.shoot = ModContent.ProjectileType<MagicSword>();
            item.shoot = 116;
            item.shootSpeed = 6f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, 8);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            position = Main.MouseWorld;
            //计算距离
            //Vector2 basePos = Vector2.Normalize(Main.MouseWorld - player.Center);
            //position = basePos * 100f + player.Center;
            position = player.Center + new Vector2(0, -0);
            //计算速度
            Vector2 baseV = Vector2.Normalize(Main.MouseWorld - position);
            baseV *= item.shootSpeed;
            int maxPro = 1;
            if (mp.PlayerClass == 5 && mp.boughtbuffList[1])
                maxPro += mp.handBloodGas / 33333 + 1;
            for (int i = 1; i <= maxPro; i++)
            {
                int param = i / 2;
                if (i % 2 == 0)
                {
                    param *= -1;
                }
                Vector2 velocity = baseV.RotatedBy(MathHelper.Pi / 180 * 5 * param);
                Vector2 newPos = position;
                newPos.X += velocity.X * 60 * 0.2f;
                newPos.Y += velocity.Y * 60 * 0.2f;
                int p = Projectile.NewProjectile(newPos.X, newPos.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                Main.projectile[p].timeLeft = 60 * 6;
                Main.projectile[p].melee = false;
                Main.projectile[p].magic = true;
                Main.projectile[p].tileCollide = false;
                Main.projectile[p].ignoreWater = true;
                Main.projectile[p].penetrate = 3;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
            if (num != -1)
            {
                string text = "";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "MagicSwordBlood", text);
                /*tooltipLine.overrideColor = Color.LimeGreen;
                tooltips.Insert(num + 1, tooltipLine);*/
                text = "追踪范围1000格";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightBlue;
                tooltips.Insert(num + 1, tooltipLine);
               /* text = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00") + "%觉醒上限";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 3, tooltipLine);
                text = "击杀敌人+" + (modPlayer.swordBloodMax / 10000 + 1) + "攻击力";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LimeGreen;
                tooltips.Insert(num + 4, tooltipLine);*/

                text = player.getDownedBoss();
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightGreen;
                tooltips.Insert(tooltips.Count, tooltipLine);
            }
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit *= 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.AddIngredient(mod.GetItem("MagicScroll"), 1);
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
