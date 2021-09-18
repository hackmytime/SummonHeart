using System;
using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Bullet
{
    public class BlackholeBulletPro : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blackhole Rocket");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            aiType = 14;
            projectile.width = 12;
            projectile.height = 16;
            projectile.aiStyle = 1;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 1;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }

        public override void Kill(int timeLeft)
        {
            int apShotFromLauncherID = projectile.GetGlobalProjectile<SummonHeartGlobalProjectile>().apShotFromLauncherID;
            Projectile.NewProjectile(projectile.position, projectile.velocity, mod.ProjectileType("BlackHole"), 0, 0f, projectile.owner, 0f, 0f);
            if (Main.netMode != 2)
            {
                Main.PlaySound(mod.GetLegacySoundSlot((SoundType)50, "Sounds/Custom/blackHole").WithVolume(0.8f), projectile.position);
            }
            AmmoHelper.explodeRocket(apShotFromLauncherID, projectile.identity, projectile.owner, default, false, false);
        }
    }
}
