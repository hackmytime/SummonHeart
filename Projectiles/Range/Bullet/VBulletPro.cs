using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Bullet
{
    public class VBulletPro : ModProjectile
    {
        public override void SetDefaults()
        {
            aiType = 14;
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 8;
            projectile.light = 1f;
            projectile.timeLeft = 600;
            projectile.extraUpdates = 1;
        }

        public override string Texture
        {
            get
            {
                return string.Format("Terraria/Projectile_{0}", 207);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
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
    }
}
