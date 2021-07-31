using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Magic
{
    public class BrokenRod : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.light = 0.1f;
            projectile.magic = true;
            drawOriginOffsetY = -20;
            drawOffsetX = -20;
            projectile.tileCollide = false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }

        public override void AI()
        {
            int TIME_DEBUFF_CHAOS = 600;
            if (projectile.soundDelay == 0 && Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > 2f)
            {
                projectile.soundDelay = 10;
                Main.PlaySound(SoundID.Item9, projectile.position);
            }
            if (Main.myPlayer == projectile.owner && projectile.ai[0] == 0f)
            {
                Player player = Main.player[projectile.owner];
                if (player.channel)
                {
                    float maxDistance = 800f;
                    Vector2 vectorToCursor = Main.MouseWorld - projectile.Center;
                    float distanceToCursor = vectorToCursor.Length();
                    if ((int)(projectile.position.X / 16.0) - (int)(player.position.X / 16f) > 20 || (int)(projectile.position.X / 16.0) - (int)(player.position.X / 16f) < -20 || (int)(projectile.position.Y / 16.0) - (int)(player.position.Y / 16f) > 20 || (int)(projectile.position.Y / 16.0) - (int)(player.position.Y / 16f) < -20)
                    {
                        int index = (int)(projectile.position.X / 16.0);
                        int index2 = (int)(projectile.position.Y / 16.0);
                        if ((Main.tile[index, index2].wall != 87 || index2 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(projectile.position, player.width, player.height))
                        {
                            if (player.chaosState)
                            {
                                projectile.Kill();
                            }
                            else
                            {
                                player.Teleport(projectile.position, 1, 0);
                                NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, projectile.position.X, projectile.position.Y, 1, 0, 0);
                                projectile.Kill();
                                player.AddBuff(88, TIME_DEBUFF_CHAOS, true);
                            }
                        }
                        else
                        {
                            projectile.Kill();
                        }
                    }
                    if (distanceToCursor > maxDistance)
                    {
                        distanceToCursor = maxDistance / distanceToCursor;
                        vectorToCursor *= distanceToCursor;
                    }
                    int num = (int)(vectorToCursor.X * 1000f);
                    int oldVelocityXBy1000 = (int)(projectile.velocity.X * 1000f);
                    int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
                    int oldVelocityYBy1000 = (int)(projectile.velocity.Y * 1000f);
                    if (num != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity = vectorToCursor;
                }
                else if (projectile.ai[0] == 0f)
                {
                    int index3 = (int)(projectile.position.X / 16.0);
                    int index4 = (int)(projectile.position.Y / 16.0);
                    if ((Main.tile[index3, index4].wall != 87 || index4 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(projectile.position, player.width, player.height))
                    {
                        if (player.chaosState)
                        {
                            projectile.Kill();
                        }
                        else
                        {
                            player.Teleport(projectile.position, 1, 0);
                            NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, projectile.position.X, projectile.position.Y, 1, 0, 0);
                            projectile.Kill();
                            player.AddBuff(88, TIME_DEBUFF_CHAOS, true);
                        }
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
                if (projectile.velocity != Vector2.Zero)
                {
                    projectile.rotation = projectile.velocity.ToRotation() + 0.7853982f;
                }
            }
        }
    }
}
