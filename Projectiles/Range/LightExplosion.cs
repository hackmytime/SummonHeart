using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range
{
    internal class LightExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 64;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.tileCollide = false;
            projectile.alpha = 1;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
        }

        public override bool PreAI()
        {
            Projectile projectile = base.projectile;
            int num = projectile.frameCounter + 1;
            projectile.frameCounter = num;
            if (num >= 2)
            {
                base.projectile.frameCounter = 0;
                Projectile projectile2 = base.projectile;
                num = projectile2.frame + 1;
                projectile2.frame = num;
                if (num >= 7)
                {
                    base.projectile.Kill();
                }
            }
            return false;
        }
    }
}
