using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range
{
    internal class HeatLaserBeam : ModProjectile
    {
        // Token: 0x17000009 RID: 9
        // (get) Token: 0x060001A1 RID: 417 RVA: 0x0000A5CE File Offset: 0x000087CE
        // (set) Token: 0x060001A2 RID: 418 RVA: 0x0000A5DD File Offset: 0x000087DD
        public float BeamLength
        {
            get
            {
                return projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x060001A3 RID: 419 RVA: 0x0000A5ED File Offset: 0x000087ED
        // (set) Token: 0x060001A4 RID: 420 RVA: 0x0000A5FC File Offset: 0x000087FC
        public float Rotation
        {
            get
            {
                return projectile.ai[1];
            }
            set
            {
                projectile.ai[1] = value;
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x060001A5 RID: 421 RVA: 0x0000A60C File Offset: 0x0000880C
        private Vector2 UnitDirection
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }
        }

        // Token: 0x060001A6 RID: 422 RVA: 0x0000A62D File Offset: 0x0000882D
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heat Laser beam");
        }

        // Token: 0x060001A7 RID: 423 RVA: 0x0000A640 File Offset: 0x00008840
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 70;
            projectile.friendly = true;
            projectile.scale = 1.8f;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.Opacity = 1f;
            PlaySounds();
            float hitscanBeamLength = PerformBeamHitscan();
            BeamLength = MathHelper.Lerp(BeamLength, hitscanBeamLength, 0.75f);
            Vector2 beamDims = new Vector2(BeamLength, projectile.width * projectile.scale);
            Color beamColor = GetOuterBeamColor();
            ProduceBeamDust(beamColor);
            if (Main.netMode != 2)
            {
                ProduceWaterRipples(beamDims);
            }
            DelegateMethods.v3_1 = beamColor.ToVector3() * 0.75f;
            Utils.PlotTileLine(projectile.Center, projectile.Center + UnitDirection * BeamLength, beamDims.Y, new Utils.PerLinePoint(DelegateMethods.CastLight));
        }

        // Token: 0x060001A9 RID: 425 RVA: 0x0000A7A4 File Offset: 0x000089A4
        private float PerformBeamHitscan()
        {
            Vector2 center = projectile.Center;
            float[] laserScanResults = new float[3];
            Collision.LaserScan(center, UnitDirection, 1f * projectile.scale, 1200f, laserScanResults);
            float averageLengthSample = 0f;
            for (int i = 0; i < laserScanResults.Length; i++)
            {
                averageLengthSample += laserScanResults[i];
            }
            return averageLengthSample / 3f;
        }

        // Token: 0x060001AA RID: 426 RVA: 0x0000A808 File Offset: 0x00008A08
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return new bool?(true);
            }
            float _ = float.NaN;
            Vector2 beamEndPos = projectile.Center + UnitDirection * BeamLength;
            return new bool?(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, beamEndPos, 22f * projectile.scale, ref _));
        }

        // Token: 0x060001AB RID: 427 RVA: 0x0000A884 File Offset: 0x00008A84
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (UnitDirection == Vector2.Zero)
            {
                return false;
            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 vector = projectile.Center.Floor() + UnitDirection * projectile.scale * 10.5f;
            Vector2 drawScale = new Vector2(projectile.scale);
            float visualBeamLength = BeamLength - 14.5f * projectile.scale * projectile.scale;
            DelegateMethods.f_1 = 1f;
            Vector2 startPosition = vector - Main.screenPosition;
            Vector2 endPosition = startPosition + UnitDirection * visualBeamLength;
            DrawBeam(spriteBatch, texture, startPosition, endPosition, drawScale, GetOuterBeamColor() * 0.75f * projectile.Opacity);
            drawScale *= 0.5f;
            DrawBeam(spriteBatch, texture, startPosition, endPosition, drawScale, GetInnerBeamColor() * 0.1f * projectile.Opacity);
            return false;
        }

        // Token: 0x060001AC RID: 428 RVA: 0x0000A9B4 File Offset: 0x00008BB4
        private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }

        // Token: 0x060001AD RID: 429 RVA: 0x0000A9E4 File Offset: 0x00008BE4
        private Color GetOuterBeamColor()
        {
            Color c = Main.hslToRgb(1f, 0.66f, 0.53f);
            c.A = 64;
            return Color.White;
        }

        // Token: 0x060001AE RID: 430 RVA: 0x0000AA10 File Offset: 0x00008C10
        private Color GetInnerBeamColor()
        {
            return Color.White;
        }

        // Token: 0x060001AF RID: 431 RVA: 0x0000AA18 File Offset: 0x00008C18
        private void ProduceBeamDust(Color beamColor)
        {
            Vector2 vector = projectile.Center + UnitDirection * (BeamLength - 14.5f * projectile.scale);
            float num = Rotation + (Main.rand.NextBool() ? 1f : -1f) * 1.5707964f;
            float startDistance = Main.rand.NextFloat(1f, 1.8f);
            float scale = Main.rand.NextFloat(0.7f, 1.1f);
            Vector2 velocity = num.ToRotationVector2() * startDistance;
            Dust dust = Dust.NewDustDirect(vector, 0, 0, 15, velocity.X, velocity.Y, 0, beamColor, scale);
            dust.color = beamColor;
            dust.noGravity = true;
            if (projectile.scale > 1f)
            {
                dust.velocity *= projectile.scale;
                dust.scale *= projectile.scale;
            }
        }

        // Token: 0x060001B0 RID: 432 RVA: 0x0000AB20 File Offset: 0x00008D20
        private void PlaySounds()
        {
            if (projectile.soundDelay <= 0)
            {
                projectile.soundDelay = 20;
                Main.PlaySound(SoundID.Item15, projectile.position);
            }
        }

        // Token: 0x060001B1 RID: 433 RVA: 0x0000AB54 File Offset: 0x00008D54
        private void ProduceWaterRipples(Vector2 beamDims)
        {
            WaterShaderData waterShaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
            float waveSine = 0.1f * (float)Math.Sin(Main.GlobalTime * 20f);
            Vector2 ripplePos = projectile.position + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(Rotation, default);
            Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
            waterShaderData.QueueRipple(ripplePos, waveData, beamDims, (RippleShape)1, Rotation);
        }

        // Token: 0x060001B2 RID: 434 RVA: 0x0000AC14 File Offset: 0x00008E14
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = (Terraria.Enums.TileCuttingContext)2;
            Utils.PerLinePoint cut = new Utils.PerLinePoint(DelegateMethods.CutTiles);
            Vector2 center = projectile.Center;
            Vector2 beamEndPos = center + UnitDirection * BeamLength;
            Utils.PlotTileLine(center, beamEndPos, projectile.width * projectile.scale, cut);
        }

        // Token: 0x0400004D RID: 77
        private const float MaxBeamLength = 1200f;

        // Token: 0x0400004E RID: 78
        private const float BeamTileCollisionWidth = 1f;

        // Token: 0x0400004F RID: 79
        private const float BeamHitboxCollisionWidth = 22f;

        // Token: 0x04000050 RID: 80
        private const int NumSamplePoints = 3;

        // Token: 0x04000051 RID: 81
        private const float BeamLengthChangeFactor = 0.75f;

        // Token: 0x04000052 RID: 82
        private const float OuterBeamOpacityMultiplier = 0.75f;

        // Token: 0x04000053 RID: 83
        private const float InnerBeamOpacityMultiplier = 0.1f;

        // Token: 0x04000054 RID: 84
        private const float BeamLightBrightness = 0.75f;

        // Token: 0x04000055 RID: 85
        private const float BeamColorHue = 1f;

        // Token: 0x04000056 RID: 86
        private const float BeamColorSaturation = 0.66f;

        // Token: 0x04000057 RID: 87
        private const float BeamColorLightness = 0.53f;

        // Token: 0x04000058 RID: 88
        private const int SoundInterval = 20;
    }
}
