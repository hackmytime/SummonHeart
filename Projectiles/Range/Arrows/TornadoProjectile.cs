using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Arrows
{
    public class TornadoProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TornadoProjectile");
            Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(386);
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.melee = false;
            projectile.ranged = false;
            projectile.magic = false;
            projectile.thrown = false;
            projectile.minion = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            int num612 = 10;
            int num613 = 15;
            float num614 = 1f;
            int num615 = 150;
            int num616 = 42;
            projectile.damage = 20;
            if (projectile.velocity.X != 0f)
            {
                projectile.direction = projectile.spriteDirection = -Math.Sign(projectile.velocity.X);
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 2)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 6)
            {
                projectile.frame = 0;
            }
            if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
            {
                projectile.localAI[0] = 1f;
                projectile.position.X = projectile.position.X + projectile.width / 2;
                projectile.position.Y = projectile.position.Y + projectile.height / 2;
                projectile.scale = (num612 + num613 - projectile.ai[1]) * num614 / (num613 + num612);
                projectile.width = (int)(num615 * projectile.scale);
                projectile.height = (int)(num616 * projectile.scale);
                projectile.position.X = projectile.position.X - projectile.width / 2;
                projectile.position.Y = projectile.position.Y - projectile.height / 2;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != -1f)
            {
                projectile.scale = (num612 + num613 - projectile.ai[1]) * num614 / (num613 + num612);
                projectile.width = (int)(num615 * projectile.scale);
                projectile.height = (int)(num616 * projectile.scale);
            }
            if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.alpha -= 30;
                if (projectile.alpha < 60)
                {
                    projectile.alpha = 60;
                }
            }
            else
            {
                projectile.alpha += 30;
                if (projectile.alpha > 150)
                {
                    projectile.alpha = 150;
                }
            }
            if (projectile.ai[0] > 0f)
            {
                projectile.ai[0] -= 1f;
            }
            if (projectile.ai[0] == 1f && projectile.ai[1] > 0f && projectile.owner == Main.myPlayer)
            {
                projectile.netUpdate = true;
                Vector2 center4 = projectile.Center;
                center4.Y -= num616 * projectile.scale / 2f;
                float num617 = (num612 + num613 - projectile.ai[1] + 1f) * num614 / (num613 + num612);
                center4.Y -= num616 * num617 / 2f;
                center4.Y += 2f;
                Projectile.NewProjectile(center4.X, center4.Y, projectile.velocity.X, projectile.velocity.Y, projectile.type, 20, projectile.knockBack, projectile.owner, 10f, projectile.ai[1] - 1f);
                int num618 = 4;
                if ((int)projectile.ai[1] % num618 == 0)
                {
                    float num621 = projectile.ai[1];
                }
            }
            if (projectile.ai[0] <= 0f)
            {
                float num622 = 0.10471976f;
                float num619 = projectile.width / 5f;
                float num620 = (float)(Math.Cos(num622 * (0.0 - projectile.ai[0])) - 0.5) * num619;
                projectile.position.X = projectile.position.X - num620 * (0f - projectile.direction);
                projectile.ai[0] -= 1f;
                num620 = (float)(Math.Cos(num622 * (0.0 - projectile.ai[0])) - 0.5) * num619;
                projectile.position.X = projectile.position.X + num620 * (0f - projectile.direction);
            }
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            base.projectile.damage = mp.tornadoDamage;
            if (!player.noKnockback && projectile.position.X / 16f <= (player.position.X + 700f) / 16f && projectile.position.X / 16f >= (player.position.X - 700f) / 16f)
            {
                if (player.position.X <= projectile.position.X + 30f)
                {
                    player.velocity.X = player.velocity.X + 0.3f;
                }
                else
                {
                    player.velocity.X = player.velocity.X - 0.3f;
                }
                if (player.position.Y <= projectile.position.Y - 200f)
                {
                    player.velocity.Y = player.velocity.Y + 0.5f;
                }
                else
                {
                    player.velocity.Y = player.velocity.Y - 0.5f;
                }
            }
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (projectile.position.X / 16f <= (Main.npc[i].position.X + 700f) / 16f && projectile.position.X / 16f >= (Main.npc[i].position.X - 700f) / 16f && Main.npc[i].type != 488)
                {
                    Main.npc[i].netUpdate = true;
                    Main.npc[i].rotation += projectile.velocity.X * 0.8f;
                    if (Main.npc[i].position.X <= projectile.position.X + 37f)
                    {
                        Main.npc[i].velocity.X = Main.npc[i].velocity.X + 0.3f;
                    }
                    else
                    {
                        Main.npc[i].velocity.X = Main.npc[i].velocity.X - 0.3f;
                    }
                    if (Main.npc[i].position.Y <= projectile.position.Y - 250f)
                    {
                        Main.npc[i].velocity.Y = Main.npc[i].velocity.Y + 0.5f;
                    }
                    else
                    {
                        Main.npc[i].velocity.Y = Main.npc[i].velocity.Y - 0.5f;
                    }
                }
                else
                {
                    Main.npc[i].rotation = 0f;
                }
            }
            /*for (int v = 0; v < Main.item.Length; v++)
            {
                if (projectile.position.X / 16f <= (Main.item[v].position.X + 700f) / 16f && projectile.position.X / 16f >= (Main.item[v].position.X - 700f) / 16f)
                {
                    if (Main.item[v].position.X <= projectile.position.X + 37f)
                    {
                        Main.item[v].velocity.X = Main.item[v].velocity.X + 0.3f;
                    }
                    else
                    {
                        Main.item[v].velocity.X = Main.item[v].velocity.X - 0.3f;
                    }
                    if (Main.item[v].position.Y <= projectile.position.Y - 200f)
                    {
                        Main.item[v].velocity.Y = Main.item[v].velocity.Y + 0.5f;
                    }
                    else
                    {
                        Main.item[v].velocity.Y = Main.item[v].velocity.Y - 0.5f;
                    }
                }
            }*/
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                Main.npc[i].rotation = 0f;
            }
        }
    }
}
