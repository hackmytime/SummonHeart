using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles
{
    public class SummonHeartGlobalProjectile : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.aiStyle == 99)
            {
                ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            }
            base.SetDefaults(projectile);
        }

        public override bool PreAI(Projectile projectile)
        {
            if (projectile.minion)
            {
                int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.GoldCoin);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                Vector2 vel = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                vel.Normalize();
                Main.dust[dust].velocity = vel * Main.rand.Next(50, 100) * 0.04f;
                Main.dust[dust].position = projectile.Center - vel * 34f;
            }
            return base.PreAI(projectile);
        }
    }
}
