using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Weapon
{
    public class Overgrowth : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overgrowth");
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 40;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 0f;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.minion = false;
            projectile.scale = 0.8f;
        }

        public override void AI()
        {
            bool flag64 = projectile.type == ModContent.ProjectileType<Overgrowth>();
            Player player = Main.player[projectile.owner];
            if (player.dead || !player.active)
            {
                projectile.Kill();
                return;
            }
            SummonHeartPlayer modPlayer = player.getModPlayer();
            if (flag64)
            {
                projectile.timeLeft = 2;
            }

            projectile.position.X = Main.player[projectile.owner].Center.X - projectile.width / 2 - 6;
            projectile.position.Y = Main.player[projectile.owner].Center.Y - projectile.height / 2 + Main.player[projectile.owner].gfxOffY - 110f;
            if (Main.player[projectile.owner].gravDir == -1f)
            {
                projectile.position.Y = projectile.position.Y + 120f;
                projectile.rotation = 3.14f;
            }
            else
            {
                //projectile.rotation = 0f;
            }

            projectile.position.X = (int)projectile.position.X;
            projectile.position.Y = (int)projectile.position.Y;
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            projectile.scale = num395 + 0.95f;

            int dist = 200;
            if (modPlayer.boughtbuffList[0])
            {
                dist += modPlayer.eyeBloodGas / 400 + 100;
            }
            for (int i = 0; i < Main.projectile.Length; ++i)
            {
                Projectile targetPro = Main.projectile[i];
                if (targetPro.active && !targetPro.friendly && targetPro.Distance(projectile.Center) <= dist)
                {
                    Projectile p = Main.projectile[i];
                    if(p != null)
                        SummonHeartMod.addSlowMap(p);
                }else if (targetPro.Distance(projectile.Center) > dist)
                {
                    SummonHeartMod.deleteSlowMap(targetPro);
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && npc.Distance(projectile.Center) < dist)
                {
                    npc.AddBuff(mod.BuffType("EyeBuff"), 120);
                    npc.velocity *= (1 - 0.33f);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * dist);
                offset.Y += (float)(Math.Cos(angle) * dist);
                Dust dust = Main.dust[Dust.NewDust(
                    projectile.Center + offset - new Vector2(4, 4), 0, 0,
                    DustID.Shadowflame, 0, 0, 100, Color.White, 1f
                    )];
                dust.velocity = projectile.velocity;
                if (Main.rand.Next(3) == 0)
                    dust.velocity += Vector2.Normalize(offset) * -5f;
                dust.noGravity = true;
            }
        }
    }
}