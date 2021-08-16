using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Melee
{
    // Token: 0x020000F5 RID: 245
    public class DragonLegacyBlue : ModProjectile
    {
        // Token: 0x060005B0 RID: 1456 RVA: 0x0001748B File Offset: 0x0001568B
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        // Token: 0x060005B1 RID: 1457 RVA: 0x00024C97 File Offset: 0x00022E97
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        // Token: 0x060005B2 RID: 1458 RVA: 0x00036140 File Offset: 0x00034340
        public override void SetDefaults()
        {
            projectile.timeLeft = 60;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.height = 72;
            projectile.width = 72;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
        }

        // Token: 0x060005B3 RID: 1459 RVA: 0x000361A4 File Offset: 0x000343A4
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.PushBlendState(BlendState.NonPremultiplied, delegate ()
            {
                Texture2D light = mod.GetTexture("Projectiles/Effects/LightTransparent");
                spriteBatch.Draw(light, projectile.Center - Main.screenPosition, null, Color.Black.MultiplyAlpha(0.75f), 0f, light.Size() / 2f, 0.5f + 0.75f * (1f - f), 0, 0f);
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor.MultiplyAlpha(f >= 0.75f ? 1f - (f - 0.75f) / 0.25f : 1f), projectile.rotation - 1.5707964f + 0.7853982f - 3.1415927f, tex.Size() / 2f, 1f, 0, 0f);
            }, null);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            var swordShader = GameShaders.Misc["SummonHeart:SwordTrail"];
            if(swordShader != null)
                swordShader.Apply(null);
            vertexStrip.PrepareStripWithProceduralPadding(projectile.oldPos, projectile.oldRot, (p) => Color.Lerp(color.MultiplyAlpha(p <= 0.1f ? p / 0.1f : 1f), Color.Lerp(color, Color.DarkViolet, 0.5f), p), (p) => 48f, -Main.screenPosition + projectile.Size / 2f, true, true);
            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            spriteBatch.PushBlendState(BlendState.NonPremultiplied, delegate ()
            {
                spriteBatch.Draw(mod.GetTexture("Projectiles/Melee/DragonLegacyBlueGlow"), projectile.Center - Main.screenPosition, null, color.MultiplyAlpha(f >= 0.75f ? 1f - (f - 0.75f) / 0.25f : 1f), projectile.rotation - 1.5707964f + 0.7853982f - 3.1415927f, tex.Size() / 2f, 1f, 0, 0f);
            }, null);
            DrawHeatEffect(spriteBatch, base.projectile.Center, base.mod.GetTexture("Projectiles/Melee/DragonLegacyBlueGlow"), null, base.projectile.rotation - 1.5707964f + 0.7853982f - 3.1415927f, Utils.Size(tex) / 2f, 1f, 0, this.color, 6f, 0, ((this.f >= 0.75f) ? (1f - (this.f - 0.75f) / 0.25f) : 1f) * 0.15f);
            return false;
        }

        public void DrawHeatEffect(SpriteBatch spriteBatch, Vector2 center, Texture2D texture, Rectangle? source, float rotation, Vector2 origin, float scale, int alpha, Color color, float amount = 24f, SpriteEffects fx = 0, float alphaMod = 0.15f)
        {
            spriteBatch.BeginBlendState(BlendState.Additive, null, false);
            for (float i = -3.1415927f; i <= 3.1415927f; i += 1.5707964f)
            {
                spriteBatch.Draw(texture, center + Utils.RotatedBy(Vector2.UnitX, (double)(i + Main.GlobalTime * 12f), default(Vector2)) * (float)((double)amount + (double)(amount / 4f) * Math.Sin((double)(Main.GlobalTime * 9f))) - Main.screenPosition, source, color.MultiplyAlpha(alphaMod).MultiplyAlpha(1f - (float)alpha / 255f), rotation, origin, scale, fx, 0f);
            }
            spriteBatch.EndBlendState(false);
        }

        // Token: 0x060005B4 RID: 1460 RVA: 0x000363DF File Offset: 0x000345DF
        public override bool CanDamage()
        {
            return f < 0.65f;
        }

        // Token: 0x060005B5 RID: 1461 RVA: 0x000363EE File Offset: 0x000345EE
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 1;
        }

        // Token: 0x060005B6 RID: 1462 RVA: 0x00036403 File Offset: 0x00034603
        private static float Ease(float f)
        {
            if (f < 1f)
            {
                return 1f - (float)Math.Pow(2.0, -10f * f);
            }
            return 1f;
        }

        // Token: 0x060005B7 RID: 1463 RVA: 0x00036430 File Offset: 0x00034630
        public override void AI()
        {
            f = 1f - projectile.timeLeft / 60f;
            if (center == default)
            {
                center = projectile.Center;
                projectile.ai[0] = (float)Main.rand.Choose(-1, 1);
                if (altSound > 3)
                {
                    altSound = 1;
                }
                Main.PlaySound(50, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, string.Format("Sounds/Items/buryTheLightHit{0}", altSound++)), 0.8f, 0f);
            }
            if (Main.netMode != 2)
            {
                color = Main.player[projectile.owner].GetModPlayer<SummonHeartPlayer>().ColorOrOther(new Color(200, 255, 248));
            }
            Vector2 vel = projectile.velocity.RotatedBy((Ease(f) - 0.5f) * projectile.ai[0] * 2.3561945f, default);
            projectile.rotation = vel.ToRotation() - 1.5707964f;
            projectile.Center = center + vel * (64f + 64f * (1f - Ease(f)));
        }

        // Token: 0x04000298 RID: 664
        private readonly VertexStrip vertexStrip = new VertexStrip();

        // Token: 0x04000299 RID: 665
        private float f;

        // Token: 0x0400029A RID: 666
        private Vector2 center;

        // Token: 0x0400029B RID: 667
        private Color color;

        // Token: 0x0400029C RID: 668
        private static int altSound = 1;
    }
}
