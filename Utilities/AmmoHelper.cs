using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace SummonHeart.Utilities
{
    internal class AmmoHelper
    {
        public static void chaseEnemy(int projid, int projType)
        {
            bool found = false;
            Projectile projectile = new Projectile();
            int projectileIndex = 0;
            foreach (Projectile p in Main.projectile)
            {
                if (p.type == projType && projid == p.identity)
                {
                    projectile = p;
                    found = true;
                    break;
                }
                projectileIndex++;
            }
            if (!found)
            {
                return;
            }
            float velVector = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
            float locAIzero = projectile.localAI[0];
            if (locAIzero == 0.0)
            {
                projectile.localAI[0] = velVector;
                locAIzero = velVector;
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            float projX = projectile.position.X;
            float projY = projectile.position.Y;
            float maxDistance = 300f;
            bool foundValidTarget = false;
            int detectedTargetID = 0;
            if (projectile.ai[1] == 0.0)
            {
                for (int index = 0; index < 200; index++)
                {
                    if (Main.npc[index].CanBeChasedBy(projectile, false) && (projectile.ai[1] == 0.0 || projectile.ai[1] == (double)(index + 1)))
                    {
                        float npcCenterX = Main.npc[index].position.X + Main.npc[index].width / 2;
                        float npcCenterY = Main.npc[index].position.Y + Main.npc[index].height / 2;
                        float npcProjDist = Math.Abs(projectile.position.X + projectile.width / 2 - npcCenterX) + Math.Abs(projectile.position.Y + projectile.height / 2 - npcCenterY);
                        if (npcProjDist < (double)maxDistance && Collision.CanHit(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2), 1, 1, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                        {
                            maxDistance = npcProjDist;
                            projX = npcCenterX;
                            projY = npcCenterY;
                            foundValidTarget = true;
                            detectedTargetID = index;
                        }
                    }
                }
                if (foundValidTarget)
                {
                    projectile.ai[1] = detectedTargetID + 1;
                }
                foundValidTarget = false;
            }
            if (projectile.ai[1] > 0.0)
            {
                int index2 = (int)(projectile.ai[1] - 1.0);
                if (Main.npc[index2].active && Main.npc[index2].CanBeChasedBy(projectile, true) && !Main.npc[index2].dontTakeDamage)
                {
                    if (Math.Abs(projectile.position.X + projectile.width / 2 - (Main.npc[index2].position.X + Main.npc[index2].width / 2)) + (double)Math.Abs(projectile.position.Y + projectile.height / 2 - (Main.npc[index2].position.Y + Main.npc[index2].height / 2)) < 1000.0)
                    {
                        foundValidTarget = true;
                        projX = Main.npc[index2].position.X + Main.npc[index2].width / 2;
                        projY = Main.npc[index2].position.Y + Main.npc[index2].height / 2;
                    }
                }
                else
                {
                    projectile.ai[1] = 0f;
                }
            }
            if (!projectile.friendly)
            {
                foundValidTarget = false;
            }
            if (foundValidTarget)
            {
                float num15 = (float)locAIzero;
                Vector2 projCenter = new Vector2((float)(projectile.position.X + projectile.width * 0.5), (float)(projectile.position.Y + projectile.height * 0.5));
                float distCenterX = projX - projCenter.X;
                float distCenterY = projY - projCenter.Y;
                double distCenter = Math.Sqrt(distCenterX * (double)distCenterX + distCenterY * (double)distCenterY);
                float num11 = (float)(num15 / distCenter);
                float num12 = distCenterX * num11;
                float num13 = distCenterY * num11;
                int num14 = 8;
                projectile.velocity.X = (float)((projectile.velocity.X * (double)(num14 - 1) + num12) / num14);
                projectile.velocity.Y = (float)((projectile.velocity.Y * (double)(num14 - 1) + num13) / num14);
            }
            Main.projectile[projectileIndex] = projectile;
        }

        public static void explodeRocket(int shot, int projid, int projtype, Color color = default, bool largeBlast = false, bool skipDamage = false)
        {
            bool found = false;
            Projectile projectile = new Projectile();
            foreach (Projectile p in Main.projectile)
            {
                if (p.type == projtype && projid == p.identity)
                {
                    projectile = p;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return;
            }
            projectile.position += new Vector2(projectile.width / 2, projectile.height / 2);
            if (largeBlast)
            {
                projectile.width = 80;
                projectile.height = 80;
            }
            else
            {
                projectile.width = 22;
                projectile.height = 22;
            }
            int runSmoke;
            int runFire;
            if (largeBlast)
            {
                runSmoke = 70;
                runFire = 40;
            }
            else
            {
                runSmoke = 30;
                runFire = 20;
            }
            projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);
            for (int i = 0; i < runSmoke; i++)
            {
                int id = Dust.NewDust(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 0, color, 1.5f);
                Main.dust[id].velocity *= 1.4f;
            }
            for (int j = 0; j < runFire; j++)
            {
                int id2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 0, default, 1.5f);
                Main.dust[id2].noGravity = true;
                Main.dust[id2].velocity *= 7f;
                id2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 0, default, 1.5f);
                Main.dust[id2].velocity *= 3f;
            }
            if (shot == 759 || shot == 760 || shot == 1946 || shot == 3546 || shot == 760)
            {
                Main.PlaySound(SoundID.Item14, projectile.position);
            }
            else
            {
                Main.PlaySound(SoundID.Item62, projectile.position);
            }
            if (skipDamage)
            {
                return;
            }
            projectile.position += new Vector2(projectile.width / 2, projectile.height / 2);
            if (largeBlast)
            {
                projectile.width = 200;
                projectile.height = 200;
            }
            else
            {
                projectile.width = 128;
                projectile.height = 128;
            }
            projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);
            projectile.Damage();
        }

        public static void createDustCircle(Vector2 position, int dustType, float radius = 10f, bool noGravity = false, bool newDustPerfect = false, int Count = 4, Color color = default(Color), int width = 8, int height = 8, Vector2 velocity = default(Vector2), double angleOffset = 0.0, int shader = 0)
        {
            double i = angleOffset;
            double increment = 2.0 / (double)Count;
            for (int done = 0; done < Count; done++)
            {
                if ((i >= 0.0 + angleOffset && i <= 0.5 + angleOffset) || (i >= 1.0 + angleOffset && i <= 1.5 + angleOffset))
                {
                    double x = (double)radius * Math.Cos(i * 3.141592653589793);
                    double y = (double)radius * Math.Sin(i * 3.141592653589793);
                    if (newDustPerfect)
                    {
                        Dust dust = Dust.NewDustPerfect(position + new Vector2((float)x, (float)y), dustType, new Vector2?(velocity), 0, color, 1f);
                        if (noGravity)
                        {
                            Main.dust[dust.dustIndex].noGravity = true;
                        }
                        if (shader != 0)
                        {
                            dust.shader = GameShaders.Armor.GetSecondaryShader(shader, Main.LocalPlayer);
                        }
                    }
                    else
                    {
                        int id = Dust.NewDust(position + new Vector2((float)x, (float)y), width, height, dustType, velocity.X, velocity.Y, 0, color, 1f);
                        if (noGravity)
                        {
                            Main.dust[id].noGravity = true;
                        }
                        if (shader != 0)
                        {
                            Main.dust[id].shader = GameShaders.Armor.GetSecondaryShader(shader, Main.LocalPlayer);
                        }
                    }
                }
                else if ((i >= 0.5 + angleOffset && i <= 1.0 + angleOffset) || (i >= 1.5 + angleOffset && i < 2.0 + angleOffset))
                {
                    double y2 = (double)radius * Math.Cos(i * 3.141592653589793);
                    double x2 = (double)radius * Math.Sin(i * 3.141592653589793);
                    if (newDustPerfect)
                    {
                        Dust dust2 = Dust.NewDustPerfect(position + new Vector2((float)x2, (float)y2), dustType, new Vector2?(velocity), 0, color, 1f);
                        if (noGravity)
                        {
                            Main.dust[dust2.dustIndex].noGravity = true;
                        }
                        if (shader != 0)
                        {
                            dust2.shader = GameShaders.Armor.GetSecondaryShader(shader, Main.LocalPlayer);
                        }
                    }
                    else
                    {
                        int id2 = Dust.NewDust(position + new Vector2((float)x2, (float)y2), width, height, dustType, velocity.X, velocity.Y, 0, color, 1f);
                        if (noGravity)
                        {
                            Main.dust[id2].noGravity = true;
                        }
                        if (shader != 0)
                        {
                            Main.dust[id2].shader = GameShaders.Armor.GetSecondaryShader(shader, Main.LocalPlayer);
                        }
                    }
                }
                i += increment;
            }
        }
    }
}
