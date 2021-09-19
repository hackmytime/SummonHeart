using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Bullet
{
    public class MaxBulletPro : ModProjectile
    {
        public override void SetDefaults()
        {
            aiType = 14;
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.light = 1f;
            projectile.timeLeft = 360;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
            projectile.extraUpdates = 1;
        }

        public override string Texture
        {
            get
            {
                return string.Format("Terraria/Projectile_{0}", 207);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 origin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int index = 0; index < projectile.oldPos.Length; index++)
            {
                Vector2 position = projectile.oldPos[index] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - index) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], position, null, color, projectile.rotation, origin, projectile.scale, 0, 0f);
            }
            return true;
        }

        public override bool PreAI()
        {
            projectile.rotation = (float)(projectile.velocity.ToRotation() + (double)MathHelper.ToRadians(90f));
            projectile.spriteDirection = projectile.direction;
            float num = (float)Math.Sqrt(Math.Pow(projectile.velocity.X, 2.0) + Math.Pow(projectile.velocity.Y, 2.0));
            float num2 = projectile.localAI[0];
            if (num2 == 0.0)
            {
                projectile.localAI[0] = num;
                num2 = num;
            }
            float num3 = projectile.position.X;
            float num4 = projectile.position.Y;
            float num5 = 650f;
            bool flag = false;
            int num6 = 0;
            if (projectile.ai[1] == 0.0)
            {
                for (int index = 0; index < 200; index++)
                {
                    if (Main.npc[index].CanBeChasedBy(projectile, false) && (projectile.ai[1] == 0.0 || projectile.ai[1] == index + 1))
                    {
                        float num7 = Main.npc[index].position.X + Main.npc[index].width / 2;
                        float num8 = Main.npc[index].position.Y + Main.npc[index].height / 2;
                        float num9 = Math.Abs(projectile.position.X + projectile.width / 2 - num7) + Math.Abs(projectile.position.Y + projectile.height / 2 - num8);
                        if (num9 < num5 && Collision.CanHit(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2), 1, 1, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                        {
                            num5 = num9;
                            num3 = num7;
                            num4 = num8;
                            flag = true;
                            num6 = index;
                        }
                    }
                }
                if (flag)
                {
                    projectile.ai[1] = num6 + 1;
                }
                flag = false;
            }
            if (projectile.ai[1] > 0.0)
            {
                int index2 = (int)(projectile.ai[1] - 1.0);
                if (Main.npc[index2].active && Main.npc[index2].CanBeChasedBy(projectile, true) && !Main.npc[index2].dontTakeDamage)
                {
                    if (Math.Abs(projectile.position.X - (Main.npc[index2].position.X + Main.npc[index2].width / 2f)) + (double)Math.Abs(projectile.position.Y - (Main.npc[index2].position.Y + Main.npc[index2].height / 2f)) < 1000.0)
                    {
                        flag = true;
                        num3 = Main.npc[index2].position.X + Main.npc[index2].width / 2f;
                        num4 = Main.npc[index2].position.Y + Main.npc[index2].height / 2f;
                    }
                }
                else
                {
                    projectile.ai[1] = 0f;
                }
            }
            if (!projectile.friendly)
            {
                flag = false;
            }
            if (flag)
            {
                float num17 = (float)num2;
                Vector2 vector2 = new Vector2(projectile.position.X, projectile.position.Y);
                float num10 = num3 - vector2.X;
                float num11 = num4 - vector2.Y;
                double num12 = Math.Sqrt(num10 * (double)num10 + num11 * (double)num11);
                float num13 = (float)(num17 / num12);
                float num14 = num10 * num13;
                float num15 = num11 * num13;
                int num16 = 8;
                projectile.velocity.X = (projectile.velocity.X * (num16 - 1) + num14) / num16;
                projectile.velocity.Y = (projectile.velocity.Y * (num16 - 1) + num15) / num16;
            }
            return false;
        }
    }
}
