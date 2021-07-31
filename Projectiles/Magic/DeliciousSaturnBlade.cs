using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Magic
{
    // Token: 0x020000F4 RID: 244
    public class DeliciousSaturnBlade : ModProjectile
    {
        // Token: 0x060005A9 RID: 1449 RVA: 0x00035A58 File Offset: 0x00033C58
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.width = 28;
            projectile.height = 28;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 500;
            projectile.damage = 100;
            projectile.aiStyle = 0;
            projectile.scale = 0.6f;
            projectile.localNPCHitCooldown = 6;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        // Token: 0x060005AA RID: 1450 RVA: 0x0002029D File Offset: 0x0001E49D
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(curColor * ((255 - projectile.alpha) / 255f));
        }

        // Token: 0x060005AB RID: 1451 RVA: 0x00035AFC File Offset: 0x00033CFC
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.BeginBlendState(BlendState.Additive, null, false);
            Texture2D tex = Main.projectileTexture[projectile.type];
            float len = projectile.oldPos.Length;
            for (int i = 0; i < projectile.oldPos.Length - 1; i++)
            {
                for (float j = 0f; j <= 1f; j += 0.1f)
                {
                    spriteBatch.Draw(tex, Vector2.Lerp(projectile.oldPos[i], projectile.oldPos[i + 1], j) + projectile.Size / 2f - Main.screenPosition, null, Color.Lerp(Color.Lerp(curColor, Color.Transparent, (float)i / len), Color.Lerp(curColor, Color.Transparent, (float)(i + 1) / len), j).MultiplyAlpha(0.5f).MultiplyAlpha((float)projectile.alpha / 255f), projectile.rotation, tex.Size() / 2f, 1f - MathHelper.Lerp((float)i / len, (float)(i + 1) / len, j), 0, 0f);
                }
            }
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, curColor.MultiplyAlpha((float)projectile.alpha / 255f), projectile.rotation, tex.Size() / 2f, 1.2f, 0, 0f);
            spriteBatch.EndBlendState(false);
            return false;
        }

        // Token: 0x060005AC RID: 1452 RVA: 0x00035CCC File Offset: 0x00033ECC
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.BeginBlendState(BlendState.Additive, null, false);
            float f = Math.Abs((projectile.timeLeft - 100) / 800f) / 0.5f;
            if (projectile.timeLeft <= 100)
                f = 0;
            Texture2D tex = mod.GetTexture("Projectiles/Effects/Light");
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, curColor.MultiplyAlpha((1f - f) * 0.25f), projectile.rotation * 0.01f, tex.Size() / 2f, 2f - f, 0, 0f);
            Texture2D flareTex = mod.GetTexture("Projectiles/Effects/Flare1");
            if (projectile.localAI[0] > 0f)
            {
                f = projectile.localAI[0] / 6f;
                spriteBatch.Draw(flareTex, projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, f * 0.3f), Main.GlobalTime * -0.1f + projectile.identity, flareTex.Size() * 0.5f, 0.1f + f * 1.7f, 0, 0f);
                spriteBatch.Draw(flareTex, projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, f * 0.6f), Main.GlobalTime / 2f * -0.1f + projectile.identity, flareTex.Size() * 0.5f, 0.1f + f * 1.1f, 0, 0f);
                spriteBatch.Draw(flareTex, projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, f * 0.9f), Main.GlobalTime * 2f * -0.1f + projectile.identity, flareTex.Size() * 0.5f, 0.1f + f * 0.6f, 0, 0f);
                projectile.localAI[0] -= 1f;
            }
            spriteBatch.EndBlendState(false);
        }

        // Token: 0x060005AD RID: 1453 RVA: 0x00035F60 File Offset: 0x00034160
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
           /* projectile.localAI[0] = 6f;
            Lighting.AddLight(target.Center, Vector3.One);
            projectile.timeLeft += 10;
            if (target.life <= 0)
            {
                Projectile projectile = Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ModContent.ProjectileType<DeliciousSaturnBlade>(), damage, knockback, Main.LocalPlayer.whoAmI, 0f, 0f);
                projectile.ai[0] = Main.LocalPlayer.whoAmI;
                projectile.netUpdate = true;
            }*/
        }

        // Token: 0x060005AE RID: 1454 RVA: 0x00035FF0 File Offset: 0x000341F0
        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer mp = player.SummonHeart();
            int range = mp.magicSwordBlood / 33 + 100;
            if(range > 400)
            {
                range = 400;
            }
            float f = Math.Abs((projectile.timeLeft - 100) / 800f) / 0.5f;
            if(projectile.timeLeft <= 100)
                f = 0;
            Vector2 desire = player.Center + new Vector2(
                (float)Math.Sin(Main.GlobalTime * 4f + projectile.whoAmI) * (64f + range * (1f - f)), 
                (float)Math.Cos(Main.GlobalTime * 4f + projectile.whoAmI) * (64f + range * (1f - f))).RotatedBy(projectile.ai[1], default);
            projectile.velocity = VectorHelper.VelocityToPoint(projectile.Center, desire, projectile.Center.Distance(desire) * 0.6f);
            projectile.rotation = Utils.ToRotation(VectorHelper.VelocityToPoint(projectile.Center, player.Center, 1f));
            projectile.alpha = (int)(255f * (1f - f));
        }

        // Token: 0x04000297 RID: 663
        private const int timeLeft = 360;
        private Color curColor = Color.Purple;
    }
}
