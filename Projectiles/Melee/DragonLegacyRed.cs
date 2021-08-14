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
    public class DragonLegacyRed : ModProjectile
    {
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.timeLeft = 90;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.height = 72;
            projectile.width = 72;
            projectile.extraUpdates = 2;
            projectile.melee = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
        }

        // Token: 0x060005B3 RID: 1459 RVA: 0x00036230 File Offset: 0x00034430
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
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            //GameShaders.Misc["SummonHeart:SwordTrail"].Apply(null);
            vertexStrip.PrepareStripWithProceduralPadding(projectile.oldPos, projectile.oldRot, (float p) => Color.Lerp(new Color(255, 252, 158).MultiplyAlpha(p <= 0.1f ? p / 0.1f : 1f), Color.Red, p), (float p) => 48f, -Main.screenPosition + projectile.Size / 2f, true, true);
            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            spriteBatch.PushBlendState(BlendState.NonPremultiplied, delegate ()
            {
                spriteBatch.Draw(mod.GetTexture("Projectiles/Melee/DragonLegacyRedGlow"), projectile.Center - Main.screenPosition, null, Color.White.MultiplyAlpha(f >= 0.75f ? 1f - (f - 0.75f) / 0.25f : 1f), projectile.rotation - 1.5707964f + 0.7853982f - 3.1415927f, tex.Size() / 2f, 1f, 0, 0f);
            }, null);
            DrawHeatEffect(spriteBatch, projectile.Center, mod.GetTexture("Projectiles/Melee/DragonLegacyRedGlow"), null, projectile.rotation - 1.5707964f + 0.7853982f - 3.1415927f, tex.Size() / 2f, 1f, 0, Color.White, 6f, 0, (f >= 0.75f ? 1f - (f - 0.75f) / 0.25f : 1f) * 0.15f);
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

        public override bool CanDamage()
        {
            return f < 0.65f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 2;
        }

        private static float Ease(float f)
        {
            if (f < 1f)
            {
                return 1f - (float)Math.Pow(2.0, -10f * f);
            }
            return 1f;
        }

        public override void AI()
        {
            f = 1f - projectile.timeLeft / 90f;
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
            Vector2 vel = projectile.velocity.RotatedBy((Ease(f) - 0.5f) * projectile.ai[0] * 2.8274333f, default);
            projectile.rotation = vel.ToRotation() - 1.5707964f;
            projectile.Center = center + vel * (64f + 96f * (1f - Ease(f)));
        }

        private readonly VertexStrip vertexStrip = new VertexStrip();

        private float f;

        private Vector2 center;

        private static int altSound;
    }
}
