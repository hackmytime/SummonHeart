using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range
{
    public class LightningBolt : ModProjectile
    {
        public int OriginX
        {
            get
            {
                return (int)projectile.ai[0];
            }
            set
            {
                projectile.ai[0] = value;
            }
        }

        public float OriginY
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

        // Token: 0x060001B8 RID: 440 RVA: 0x0000AC98 File Offset: 0x00008E98
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        // Token: 0x060001B9 RID: 441 RVA: 0x0000ACF4 File Offset: 0x00008EF4
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            DrawnBolt drawnBolt = bolt;
            if (drawnBolt != null)
            {
                drawnBolt.Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        // Token: 0x060001BA RID: 442 RVA: 0x0000AD6C File Offset: 0x00008F6C
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.damage = 0;
            projectile.penetrate = -1;
            for (int dust_count = 0; dust_count < 10; dust_count++)
            {
                Dust.NewDustDirect(target.Center - new Vector2(5f, 5f), 10, 10, 87, 0f, 0f, 0, default, 0.6f).firstFrame = true;
            }
        }

        // Token: 0x060001BB RID: 443 RVA: 0x0000ADE4 File Offset: 0x00008FE4
        public override void AI()
        {
            if (bolt == null)
            {
                bolt = new DrawnBolt(new Vector2(OriginX, OriginY), projectile.position);
                return;
            }
            bolt.Update();
            if (bolt.IsComplete)
            {
                projectile.Kill();
            }
        }

        // Token: 0x04000059 RID: 89
        private DrawnBolt bolt;

        // Token: 0x0200005D RID: 93
        private class DrawnBolt
        {
            // Token: 0x1700000E RID: 14
            // (get) Token: 0x060001BD RID: 445 RVA: 0x0000AE45 File Offset: 0x00009045
            // (set) Token: 0x060001BE RID: 446 RVA: 0x0000AE4D File Offset: 0x0000904D
            public float Alpha { get; set; }

            // Token: 0x1700000F RID: 15
            // (get) Token: 0x060001BF RID: 447 RVA: 0x0000AE56 File Offset: 0x00009056
            // (set) Token: 0x060001C0 RID: 448 RVA: 0x0000AE5E File Offset: 0x0000905E
            public float FadeOutRate { get; set; }

            // Token: 0x17000010 RID: 16
            // (get) Token: 0x060001C1 RID: 449 RVA: 0x0000AE67 File Offset: 0x00009067
            // (set) Token: 0x060001C2 RID: 450 RVA: 0x0000AE6F File Offset: 0x0000906F
            public Color Tint { get; set; }

            // Token: 0x17000011 RID: 17
            // (get) Token: 0x060001C3 RID: 451 RVA: 0x0000AE78 File Offset: 0x00009078
            public bool IsComplete
            {
                get
                {
                    return Alpha <= 0f;
                }
            }

            // Token: 0x060001C4 RID: 452 RVA: 0x0000AE8A File Offset: 0x0000908A
            public DrawnBolt(Vector2 source, Vector2 dest) : this(source, dest, pickColor(), Main.rand.NextFloat(0.02f, 0.05f))
            {
            }

            // Token: 0x060001C5 RID: 453 RVA: 0x0000AEAD File Offset: 0x000090AD
            public DrawnBolt(Vector2 source, Vector2 dest, Color color, float fadeRate)
            {
                Segments = CreateBolt(source, dest, 2f);
                Tint = color;
                Alpha = 1f;
                FadeOutRate = fadeRate;
            }

            // Token: 0x060001C6 RID: 454 RVA: 0x0000AEEC File Offset: 0x000090EC
            private static Color pickColor()
            {
                return ColorHelper.getRandomColor();
                /*switch (Main.rand.Next(0, 4))
                {
                    case 0:
                        return Color.Yellow;
                    case 1:
                        return Color.Aquamarine;
                    case 2:
                        return Color.BlueViolet;
                    case 3:
                        return Color.LightGoldenrodYellow;
                    default:
                        return Color.AliceBlue;
                }*/
            }

            // Token: 0x060001C7 RID: 455 RVA: 0x0000AF3C File Offset: 0x0000913C
            public void Draw(SpriteBatch spriteBatch)
            {
                if (Alpha <= 0f)
                {
                    return;
                }
                foreach (Line line in Segments)
                {
                    line.Draw(spriteBatch, Tint * Alpha);
                }
            }

            // Token: 0x060001C8 RID: 456 RVA: 0x0000AFAC File Offset: 0x000091AC
            public virtual void Update()
            {
                Alpha -= FadeOutRate;
                foreach (Line line in Segments)
                {
                    Lighting.AddLight(line.A, Tint.ToVector3() * Alpha * 0.7f);
                }
            }

            // Token: 0x060001C9 RID: 457 RVA: 0x0000B038 File Offset: 0x00009238
            protected static List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness)
            {
                List<Line> results = new List<Line>();
                Vector2 tangent = dest - source;
                Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
                float length = tangent.Length();
                List<float> positions = new List<float>
                {
                    0f
                };
                int i = 0;
                while (i < length / 4f)
                {
                    positions.Add(Main.rand.NextFloat(0f, 1f));
                    i++;
                }
                positions.Sort();
                Vector2 prevPoint = source;
                float prevDisplacement = 0f;
                for (int j = 1; j < positions.Count; j++)
                {
                    float pos = positions[j];
                    float scale = length * 0.0125f * (pos - positions[j - 1]);
                    float envelope = pos > 0.95f ? 20f * (1f - pos) : 1f;
                    float displacement = Main.rand.NextFloat(-80f, 80f);
                    displacement -= (displacement - prevDisplacement) * (1f - scale);
                    displacement *= envelope;
                    Vector2 point = source + pos * tangent + displacement * normal;
                    results.Add(new Line(prevPoint, point, thickness));
                    prevPoint = point;
                    prevDisplacement = displacement;
                }
                results.Add(new Line(prevPoint, dest, thickness));
                return results;
            }

            // Token: 0x0400005A RID: 90
            public List<Line> Segments = new List<Line>();
        }

        // Token: 0x0200005E RID: 94
        public class Line
        {
            // Token: 0x060001CA RID: 458 RVA: 0x000026D0 File Offset: 0x000008D0
            public Line()
            {
            }

            // Token: 0x060001CB RID: 459 RVA: 0x0000B19C File Offset: 0x0000939C
            public Line(Vector2 a, Vector2 b, float thickness = 1f)
            {
                A = a;
                B = b;
                Thickness = thickness;
            }

            // Token: 0x060001CC RID: 460 RVA: 0x0000B1BC File Offset: 0x000093BC
            public void Draw(SpriteBatch spriteBatch, Color color)
            {
                LIGHTNING_END_PIECE = SummonHeartMod.Instance.GetTexture(_LIGHTNING_END_PIECE);
                LIGHTNING_CENTER_SEGMENT = SummonHeartMod.Instance.GetTexture(_LIGHTNING_CENTER_SEGMENT);
                Vector2 tangent = B - A;
                float rotation = (float)Math.Atan2(tangent.Y, tangent.X);
                float thicknessScale = Thickness / 8f;
                Vector2 capOrigin = new Vector2((float)LIGHTNING_END_PIECE.Width, (float)LIGHTNING_END_PIECE.Height / 2f);
                Vector2 middleOrigin = new Vector2(0f, (float)LIGHTNING_CENTER_SEGMENT.Height / 2f);
                Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);
                spriteBatch.Draw(LIGHTNING_CENTER_SEGMENT, A - Main.screenPosition, null, color, rotation, middleOrigin, middleScale, 0, 0f);
                spriteBatch.Draw(LIGHTNING_END_PIECE, A - Main.screenPosition, null, color, rotation, capOrigin, thicknessScale, 0, 0f);
                spriteBatch.Draw(LIGHTNING_END_PIECE, B - Main.screenPosition, null, color, rotation + 3.1415927f, capOrigin, thicknessScale, 0, 0f);
            }

            public Vector2 A;

            public Vector2 B;

            public float Thickness;

            private const string _LIGHTNING_CENTER_SEGMENT = "GFX/Effects/LightningCenterSegment";

            public static Texture2D LIGHTNING_CENTER_SEGMENT;

            private const string _LIGHTNING_END_PIECE = "GFX/Effects/LightningEndPiece";

            public static Texture2D LIGHTNING_END_PIECE;
        }
    }
}
