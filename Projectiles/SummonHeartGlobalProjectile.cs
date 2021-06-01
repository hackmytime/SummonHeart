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
    }
}
