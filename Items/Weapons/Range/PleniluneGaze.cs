using System;
using Microsoft.Xna.Framework;
using SummonHeart;
using SummonHeart.Projectiles.Magic;
using SummonHeart.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Weapons.Range
{
    // Token: 0x02000093 RID: 147
    public class PleniluneGaze : ModItem
    {
        // Token: 0x0600043C RID: 1084 RVA: 0x00045BBA File Offset: 0x00043DBA
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Delicious Saturn");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔力之源");
            Tooltip.SetDefault("Creates a whirlwind of blades upon hit\nEach blade increases your defense");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "炼体八境·武道巅峰·远古魔神临死之前碎裂不朽右臂所铸造" +
                "\n魔神之子的护道传承武器，唯魔神之子可用精血召唤使用" +
                "\n众生之怨：不受任何伤害加成，无法附魔，无法多重施法" +
                "\n弑神之力：击杀任意生物增加攻击力，然受觉醒上限限制。" +
                "\n魔源觉醒：击杀强者摄其血肉灵魂增强魔力之源，可突破觉醒上限。" +
                "\n破灭法则：暴击几率翻倍，并且暴击伤害翻倍");
        }

        // Token: 0x0600043D RID: 1085 RVA: 0x00045BE8 File Offset: 0x00043DE8
        public override void SetDefaults()
        {
            item.damage = 86;
            item.ranged = true;
            item.width = 65;
            item.height = 70;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4f;
            item.rare = 9;
            item.autoReuse = true;
            item.channel = true;
            item.shoot = 10;
            item.shootSpeed = 15f;
            item.useAmmo = AmmoID.Arrow;
        }

        // Token: 0x0600043E RID: 1086 RVA: 0x00002DE5 File Offset: 0x00000FE5
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        // Token: 0x0600043F RID: 1087 RVA: 0x00045CBC File Offset: 0x00043EBC
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.statMana < 90)
                {
                    return false;
                }
                float launchSpeed = -10f;
                Vector2 backstepVelocity = Vector2.Normalize(Main.MouseWorld - player.Center) * launchSpeed;
                player.velocity = backstepVelocity;
                player.statMana -= 90;
                player.manaRegenDelay = 220;
                Main.PlaySound(SoundID.Item29, player.position);
                Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, 0f, 0f, ModContent.ProjectileType<DeliciousSaturnBlade>(), 0, 0f, player.whoAmI, 0f, 0f);
                for (int d = 0; d < 22; d++)
                {
                    Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
                for (int d2 = 0; d2 < 12; d2++)
                {
                    Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
                for (int d3 = 0; d3 < 88; d3++)
                {
                    Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
            }
            return true;
        }

        // Token: 0x06000440 RID: 1088 RVA: 0x00045EC8 File Offset: 0x000440C8
        public override void HoldItem(Player player)
        {
            float launchSpeed = 2f + (int)Math.Round(player.GetModPlayer<SummonHeartPlayer>().magicCharge / 10f);
            Vector2 arrowVelocity = Vector2.Normalize(Main.MouseWorld - player.Center) * launchSpeed;
            if (player.altFunctionUse != 2)
            {
                if (player.channel)
                {
                    item.useTime = 0;
                    item.useAnimation = 0;
                    player.GetModPlayer<SummonHeartPlayer>().magicChargeActive = true;
                    player.GetModPlayer<SummonHeartPlayer>().magicCharge += 1f;
                    if (player.GetModPlayer<SummonHeartPlayer>().magicCharge == 1f)
                    {
                        Main.PlaySound(50, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot((SoundType)50, "Sounds/Custom/bowstring"), 0.5f, 0f);
                    }
                    if (player.GetModPlayer<SummonHeartPlayer>().magicCharge == 99f)
                    {
                        for (int d = 0; d < 22; d++)
                        {
                            Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                        }
                        for (int d2 = 0; d2 < 12; d2++)
                        {
                            Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                        }
                        for (int d3 = 0; d3 < 88; d3++)
                        {
                            Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                        }
                        Main.PlaySound(SoundID.Item29, player.position);
                    }
                    if (player.GetModPlayer<SummonHeartPlayer>().magicCharge < 100f)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            Vector2 offset = default;
                            double angle = Main.rand.NextDouble() * 2.0 * 3.141592653589793;
                            offset.X += (float)(Math.Sin(angle) * (100f - player.GetModPlayer<SummonHeartPlayer>().magicCharge));
                            offset.Y += (float)(Math.Cos(angle) * (100f - player.GetModPlayer<SummonHeartPlayer>().magicCharge));
                            Dust dust = Dust.NewDustPerfect(player.MountedCenter + offset, 20, new Vector2?(player.velocity), 200, default, 0.5f);
                            dust.fadeIn = 0.1f;
                            dust.noGravity = true;
                        }
                        Vector2 vector = new Vector2(Main.rand.Next(-28, 28) * -9.88f, Main.rand.Next(-28, 28) * -9.88f);
                        Dust dust2 = Main.dust[Dust.NewDust(player.MountedCenter + vector, 1, 1, 20, 0f, 0f, 255, new Color(0.8f, 0.4f, 1f), 0.8f)];
                        dust2.velocity = -vector / 12f;
                        dust2.velocity -= player.velocity / 8f;
                        dust2.noLight = true;
                        dust2.noGravity = true;
                        return;
                    }
                    Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-5, 5), 0f + Main.rand.Next(-5, 5), 150, default, 0.8f);
                    return;
                }
                else
                {
                    item.useTime = 40;
                    item.useAnimation = 40;
                    if (player.GetModPlayer<SummonHeartPlayer>().magicCharge >= 100f)
                    {
                        Main.PlaySound(SoundID.Item9, player.position);
                        Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, arrowVelocity.X, arrowVelocity.Y, ModContent.ProjectileType<DeliciousSaturnBlade>(), 140, 4f, player.whoAmI, 0f, 0f);
                        player.GetModPlayer<SummonHeartPlayer>().magicChargeActive = false;
                        player.GetModPlayer<SummonHeartPlayer>().magicCharge = 0f;
                        ammoConsumed = false;
                        return;
                    }
                    if (player.GetModPlayer<SummonHeartPlayer>().magicCharge > 0f)
                    {
                        Main.PlaySound(SoundID.Item5, player.position);
                        player.GetModPlayer<SummonHeartPlayer>().magicChargeActive = false;
                        player.GetModPlayer<SummonHeartPlayer>().magicCharge = 0f;
                        ammoConsumed = false;
                        Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, arrowVelocity.X, arrowVelocity.Y, 120, 86 + (int)Math.Round(player.GetModPlayer<SummonHeartPlayer>().magicCharge / 10f), 4 + (int)Math.Round(player.GetModPlayer<SummonHeartPlayer>().magicCharge / 10f), player.whoAmI, 0f, 0f);
                    }
                }
            }
        }

        // Token: 0x06000441 RID: 1089 RVA: 0x0004643E File Offset: 0x0004463E
        public override bool ConsumeAmmo(Player player)
        {
            if (ammoConsumed)
            {
                return false;
            }
            ammoConsumed = true;
            return true;
        }

        // Token: 0x06000442 RID: 1090 RVA: 0x00041319 File Offset: 0x0003F519
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            bool channel = player.channel;
            return false;
        }

        // Token: 0x040002D3 RID: 723
        private bool ammoConsumed;
    }
}
