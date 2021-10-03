using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Dusts;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range
{
    public class Judgement : ModProjectile
    {
        public float Charge
        {
            get
            {
                return projectile.localAI[0];
            }
            set
            {
                projectile.localAI[0] = value;
            }
        }

        public bool IsAtMaxCharge
        {
            get
            {
                return Charge == 40f;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Judgement");
        }

        public override void SetDefaults()
        {
            projectile.width = 0;
            projectile.height = 0;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 160;
            projectile.magic = true;
            projectile.hide = true;
            projectile.localNPCHitCooldown = 3;
            projectile.usesLocalNPCImmunity = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!IsAtMaxCharge)
            {
                scaled = 1f;
                MOVE_DISTANCE = 4f;
            }
            else if (IsAtMaxCharge && projectile.timeLeft <= 80)
            {
                scaled -= 0.06f;
                MOVE_DISTANCE -= 0.24f;
            }
            else if (animation == 0)
            {
                scaled = 5.5f;
                animation = 1;
                MOVE_DISTANCE = 20f;
            }
            else if (animation == 1)
            {
                scaled = 5f;
                animation = 0;
                MOVE_DISTANCE = 20f;
            }
            if (scaled <= 0f)
            {
                projectile.Kill();
            }
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], position, spriterotation, 10f, projectile.damage, -1.57f, 1f * scaled, 1000f, Color.White, (int)MOVE_DISTANCE);
            return false;
        }

        // Token: 0x060000AE RID: 174 RVA: 0x00007814 File Offset: 0x00005A14
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default, int transDist = 0)
        {
            float r = unit.ToRotation() + rotation;
            Vector2 origin = Vector2.Zero;
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition, new Rectangle?(new Rectangle(0, 26, 28, 26)), i < transDist ? Color.Transparent : c, r, new Vector2(14f, 13f), scale, 0, 0f);
            }
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, 28, 26)), Color.White, r, new Vector2(14f, 13f), scale, 0, 0f);
        }

        // Token: 0x060000AF RID: 175 RVA: 0x000078F8 File Offset: 0x00005AF8
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!IsAtMaxCharge)
            {
                return new bool?(false);
            }
            Vector2 unit = spriterotation;
            float point = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), position, position + unit * Distance, beamWidth, ref point));
        }

        // Token: 0x060000B0 RID: 176 RVA: 0x0000795C File Offset: 0x00005B5C
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<LightExplosion>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x0000799C File Offset: 0x00005B9C
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (timer == 0)
            {
                int num233 = (int)(Main.mouseX + Main.screenPosition.X) / 16;
                int num234 = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
                if (player.gravDir == -1f)
                {
                    num234 = (int)(Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16;
                }
                while (num234 < Main.maxTilesY && Main.tile[num233, num234] != null && !WorldGen.SolidTile2(num233, num234) && Main.tile[num233 - 1, num234] != null && !WorldGen.SolidTile2(num233 - 1, num234) && Main.tile[num233 + 1, num234] != null && !WorldGen.SolidTile2(num233 + 1, num234))
                {
                    num234++;
                }
                projectile.position = new Vector2(Main.mouseX + Main.screenPosition.X, num234 * 16);
                position = projectile.position;
                timer--;
            }
            if (IsAtMaxCharge && projectile.timeLeft <= 80)
            {
                beamWidth -= 0.5f;
            }
            else
            {
                beamWidth = 100f;
            }
            UpdatePlayer(player);
            ChargeLaser(player);
            SetLaserPosition(player);
            SpawnDusts(player);
            CastLights();
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x00007B14 File Offset: 0x00005D14
        private void SpawnDusts(Player player)
        {
            Vector2 unit = position * -1f;
            if (!IsAtMaxCharge)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 dustVel = new Vector2(1f, 0f).RotatedBy(Main.rand.NextFloat(1.57f, 1.57f) + (Main.rand.Next(2) == 0 ? -1f : 1f) * 1.57f, default);
                    Dust dust = Main.dust[Dust.NewDust(new Vector2(position.X, position.Y), 0, 0, 226, dustVel.X * 10f, dustVel.Y * 10f, 0, Color.White, 1f)];
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                    dust.shader = GameShaders.Armor.GetSecondaryShader(64, Main.LocalPlayer);
                    Dust dust2 = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 31, -unit.X * Distance, -unit.Y * Distance, 0, default, 1f);
                    dust2.fadeIn = 0f;
                    dust2.noGravity = true;
                    dust2.scale = 0.88f;
                    dust2.color = Color.White;
                    dust2.shader = GameShaders.Armor.GetSecondaryShader(64, Main.LocalPlayer);
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector2 dustVel2 = new Vector2(1f, 0f).RotatedBy(Main.rand.NextFloat(1.57f, 1.57f) + (Main.rand.Next(2) == 0 ? -1f : 1f) * 1.57f, default);
                    Dust dust3 = Main.dust[Dust.NewDust(new Vector2(position.X, position.Y), 0, 0, 226, dustVel2.X * 10f, dustVel2.Y * 10f, 0, Color.White, 1f)];
                    dust3.noGravity = true;
                    dust3.scale = 1.2f;
                    dust3.shader = GameShaders.Armor.GetSecondaryShader(64, Main.LocalPlayer);
                    Dust dust4 = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 31, -unit.X * Distance, -unit.Y * Distance, 0, default, 1f);
                    dust4.fadeIn = 0f;
                    dust4.noGravity = true;
                    dust4.scale = 0.88f;
                    dust4.color = Color.White;
                    dust4.shader = GameShaders.Armor.GetSecondaryShader(64, Main.LocalPlayer);
                    Dust dust5 = Main.dust[Dust.NewDust(new Vector2(position.X, position.Y - Distance + offDistance - 65f), 0, 0, 226, dustVel2.X * 10f, dustVel2.Y * 10f, 0, Color.White, 1f)];
                    dust5.noGravity = true;
                    dust5.scale = 1.2f;
                    dust4.shader = GameShaders.Armor.GetSecondaryShader(64, Main.LocalPlayer);
                    Dust dust6 = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 31, -unit.X * Distance, -unit.Y * Distance, 0, default, 1f);
                    dust6.fadeIn = 0f;
                    dust6.noGravity = true;
                    dust6.scale = 0.88f;
                    dust6.color = Color.White;
                    dust4.shader = GameShaders.Armor.GetSecondaryShader(64, Main.LocalPlayer);
                }
            }
            if (true)
            {
                timer2 += 2;
            }
            else
            {
                timer2++;
            }
            if (timer2 == 40)
            {
                Main.PlaySound(mod.GetLegacySoundSlot((SoundType)50, "Sounds/Custom/Spells/Judgement").WithPitchVariance(1f).WithVolume(0.2f), projectile.position);
                for (int k = 0; k < 85; k++)
                {
                    Vector2 dustVel3 = new Vector2(1f, 0f).RotatedBy(Main.rand.NextFloat(1.57f, 2.57f) + (Main.rand.Next(2) == 0 ? -1.6f : 1f) * 1.57f, default);
                    float rand = Main.rand.NextFloat(5f, 20f);
                    Dust dust7 = Main.dust[Dust.NewDust(new Vector2(position.X, position.Y), 0, 0, ModContent.DustType<LightFeather>(), dustVel3.X * rand, dustVel3.Y * rand, 0, default, 1f)];
                    dust7.noGravity = true;
                    dust7.noLight = true;
                    dust7.scale = 1.2f;
                    dust7.alpha += 2;
                    Dust dust8 = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 31, -unit.X * Distance, -unit.Y * Distance, 0, default, 1f);
                    dust8.fadeIn = 0f;
                    dust8.noGravity = true;
                    dust8.scale = 0.88f;
                    dust8.alpha += 2;
                    dust8.color = Color.Cyan;
                }
            }
            if (true)
            {
                timer3 += 2;
            }
            else
            {
                timer3++;
            }
            if (timer3 >= 40)
            {
                if (Main.GameUpdateCount % 2U == 0U)
                {
                    Dust dust9 = Dust.NewDustPerfect(new Vector2((float)(position.X + WaveLength * Math.Sin(increaseY / WaveFrequency)), position.Y - increaseY), ModContent.DustType<LightBubble>(), new Vector2?(new Vector2(0f, 0f)), 0, default, 1f);
                    dust9.noGravity = true;
                    dust9.scale = 1.2f;
                    Dust dust10 = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 31, -unit.X * Distance, -unit.Y * Distance, 0, default, 1f);
                    dust10.fadeIn = 0f;
                    dust10.noGravity = true;
                    dust10.scale = 0.88f;
                    dust10.color = Color.Cyan;
                }
                if (increaseY < Distance)
                {
                    increaseY += 10f;
                }
            }
            if (timer3 >= 40)
            {
                if (Main.GameUpdateCount % 2U == 0U)
                {
                    Dust dust11 = Dust.NewDustPerfect(new Vector2((float)(position.X - WaveLength * Math.Sin(increaseY2 / WaveFrequency)), position.Y - increaseY2), ModContent.DustType<LightBubble>(), new Vector2?(new Vector2(0f, 0f)), 0, default, 1f);
                    dust11.noGravity = true;
                    dust11.scale = 1.2f;
                    Dust dust12 = Dust.NewDustDirect(new Vector2(position.X, position.Y), 0, 0, 31, -unit.X * Distance, -unit.Y * Distance, 0, default, 1f);
                    dust12.fadeIn = 0f;
                    dust12.noGravity = true;
                    dust12.scale = 0.88f;
                    dust12.color = Color.Cyan;
                }
                if (increaseY2 < Distance)
                {
                    increaseY2 += 10f;
                }
            }
        }

        private void SetLaserPosition(Player player)
        {
            Distance = MOVE_DISTANCE;
            while (Distance <= 2500f)
            {
                Vector2 start = position + spriterotation * Distance;
                if (!Collision.CanHit(position, 1, 1, start, 1, 1))
                {
                    if (!IsAtMaxCharge)
                    {
                        Distance -= 0f;
                        return;
                    }
                    if (IsAtMaxCharge && projectile.timeLeft <= 80)
                    {
                        Distance -= 50f - offDistance;
                        offDistance += 0.6f;
                        return;
                    }
                    Distance -= 50f;
                    return;
                }
                else
                {
                    Distance += 1f;
                }
            }
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x00008490 File Offset: 0x00006690
        private void ChargeLaser(Player player)
        {
            if (Charge < 40f)
            {
                if (true)
                {
                    Charge += 2f;
                    return;
                }
                float charge = Charge;
                Charge = charge + 1f;
            }
        }

        private void UpdatePlayer(Player player)
        {
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = Main.MouseWorld - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            int direction = projectile.direction;
            player.heldProj = projectile.whoAmI;
        }

        private void CastLights()
        {
            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(position, position + spriterotation * (Distance - MOVE_DISTANCE), 50f, new Utils.PerLinePoint(DelegateMethods.CastLight));
        }

        // Token: 0x0400007F RID: 127
        private const float MAX_CHARGE = 40f;

        // Token: 0x04000080 RID: 128
        public float MOVE_DISTANCE = 20f;

        // Token: 0x04000081 RID: 129
        public float Distance = 2500f;

        // Token: 0x04000082 RID: 130
        private float scaled = 5f;

        // Token: 0x04000083 RID: 131
        private float increaseY;

        // Token: 0x04000084 RID: 132
        private float increaseY2;

        // Token: 0x04000085 RID: 133
        private float WaveFrequency = 70f;

        // Token: 0x04000086 RID: 134
        private float WaveLength = 100f;

        // Token: 0x04000087 RID: 135
        private float beamWidth = 100f;

        // Token: 0x04000088 RID: 136
        private float offDistance = 0.6f;

        // Token: 0x04000089 RID: 137
        private Vector2 position;

        // Token: 0x0400008A RID: 138
        private Vector2 spriterotation = new Vector2(0f, -1f);

        // Token: 0x0400008B RID: 139
        private int timer;

        // Token: 0x0400008C RID: 140
        private int timer2;

        // Token: 0x0400008D RID: 141
        private int timer3;

        // Token: 0x0400008E RID: 142
        private int animation;
    }
}
