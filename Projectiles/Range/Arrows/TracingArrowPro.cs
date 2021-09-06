using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Arrows
{
    internal class TracingArrowPro : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.timeLeft = 300;
            projectile.arrow = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.ranged = true;
            projectile.penetrate = 8;
            projectile.aiStyle = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1;
            aiType = 14;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TracingArrowPro");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 24;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }


       /* public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 1;
            projectile.ai[1] = 1f;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            projectile.ai[1] = 1f;
        }*/

        public override bool CanDamage()
        {
            return projectile.timeLeft > 25;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float lifetime = (base.projectile.timeLeft < 30) ? ((float)base.projectile.timeLeft / 30f) : 1f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            GameShaders.Misc["SummonHeart:SoftTrail"].Apply(null);
            this.vertexStrip.PrepareStripWithProceduralPadding(base.projectile.oldPos, base.projectile.oldRot, (float p) => Color.Lerp(Color.Lime.MultiplyAlpha(lifetime * ((p <= 0.2f) ? (p / 0.2f) : 1f)), Color.IndianRed.MultiplyAlpha(0f), p), (float p) => 4f * ((this.projectile.timeLeft > 50) ? this.projectile.scale : (this.projectile.scale * (float)this.projectile.timeLeft * 0.02f)) * (1f - p), -Main.screenPosition + base.projectile.Size / 2f, true, true);
            this.vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
            spriteBatch.BeginBlendState(BlendState.Additive, null, false);
            Texture2D lightTex = base.mod.GetTexture("Projectiles/Effects/Light");
            spriteBatch.Draw(lightTex, base.projectile.Center - Main.screenPosition, null, Color.FromNonPremultiplied(173, 243, 126, 100), 0f, Utils.Size(lightTex) / 2f, ((base.projectile.timeLeft > 25) ? base.projectile.scale : (base.projectile.scale * (float)base.projectile.timeLeft * 0.04f)) * (0.3f + 0.1f * (float)Math.Sin(Main.time / 10.0)), 0, 0f);
            spriteBatch.EndBlendState(false);
            spriteBatch.Draw(Main.projectileTexture[base.projectile.type], base.projectile.Center - Main.screenPosition, null, Color.FromNonPremultiplied(255, 255, 255, (int)(lifetime * 255f)), base.projectile.rotation + 1.5707964f, new Vector2(7f), base.projectile.scale, 0, 0f);
            return false;
        }

        public override void AI()
        {
            NPC npc = Helper.GetNearestNPC(projectile.position, null, float.MaxValue);
            if (npc != null && Vector2.Distance(npc.position, projectile.position) < 800f && npc.active && npc.CanBeChasedBy(projectile, false) && projectile.ai[1] == 0f)
            {
                projectile.Navigate(npc.Center, 30f, 40f);
            }
            if (projectile.timeLeft >= 110)
            {
                projectile.alpha = 25 * (projectile.timeLeft - 110);
            }
            if (projectile.ai[1] == 1f)
            {
                if (projectile.timeLeft > 25)
                {
                    projectile.timeLeft = 25;
                }
                projectile.velocity *= 0.8f;
            }
            else
            {
                projectile.localAI[1] = projectile.velocity.ToRotation();
            }
            projectile.rotation = projectile.localAI[1];
        }

        private readonly VertexStrip vertexStrip = new VertexStrip();
    }
}
