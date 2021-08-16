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
                "\n众生之怨：不受任何伤害暴击加成，无法附魔，减少2倍攻速加成" +
                "\n弑神之力：击杀任意生物增加攻击力，然受觉醒上限限制。" +
                "\n空间法则：攻击范围内攻击无视距离" +
                "\n魔剑觉醒：击杀强者摄其血肉灵魂修复剑身，可突破觉醒上限。");
        }

        public override void SetDefaults()
        {
            item.useStyle = 3;
            item.shootSpeed = 1f;
            item.shoot = ModContent.ProjectileType<DragonLegacyRed>();
            item.noUseGraphic = true;
            item.melee = true;
            item.noMelee = true;
            item.damage = 1; //DPS 126
            item.knockBack = 3;
            item.autoReuse = true;
            //item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 22;
            item.rare = -12;
            item.value = Item.sellPrice(999, 0, 0, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            position = Main.MouseWorld;
            //计算距离
            float dist = Vector2.Distance(position, player.Center);
            float attackRange = 200f + mp.swordBlood / 20;
            if (attackRange > 500)
                attackRange = 500f;
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

        /*public override void HoldItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            attackRange = 500f + mp.swordBlood / 20;
            if (attackRange > 500)
                attackRange = 500f;
            float speedFactor = Math.Max(Math.Abs(player.velocity.X), Math.Abs(player.velocity.Y / 2));
            float radius =  Math.Max(64, attackRange);
            for (int i = 0; i < 10; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * radius);
                offset.Y += (float)(Math.Cos(angle) * radius);

                Dust d = Dust.NewDustPerfect(player.Center + offset, dustId, player.velocity, 200, default, 0.3f);
                d.fadeIn = 0.5f;
                d.noGravity = true;
            }
            base.HoldItem(player);
        }*/

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
                tooltips[num + 1].overrideColor = Color.LimeGreen;
                tooltips[num + 1].text = str + (GameCulture.Chinese.IsActive ? "觉醒度" : "Arousal Level");
                string text;
                float attackRange = 200f + modPlayer.swordBlood / 20;
                if (attackRange > 500)
                    attackRange = 500f;
                text = "攻击范围 " + attackRange + "格";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightGreen;
                tooltips.Insert(num + 2, tooltipLine);
                text = "击杀敌人+" + (modPlayer.swordBloodMax / 10000 + 1) + "攻击力";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.LightGreen;
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
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if(mp.PlayerClass == 1)
            {
                crit = 50;
            }
            else if(mp.PlayerClass == 4)
            {
                crit = mp.angerResourceCurrent;
            }
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            damage = modPlayer.swordBlood;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (crit)
            {
                damage *= 2;
            }
        }
    }
}
