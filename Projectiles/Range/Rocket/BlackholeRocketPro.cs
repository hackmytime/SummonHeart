using System;
using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Rocket
{
    public class BlackholeRocketPro : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blackhole Rocket");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 14;
            projectile.aiStyle = 16;
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

        public override void AI()
        {
            int apShotFromLauncherID = projectile.GetGlobalProjectile<SummonHeartGlobalProjectile>().apShotFromLauncherID;
            if (apShotFromLauncherID == 759)
            {
                projectile.velocity = projectile.oldVelocity;
                if (Math.Abs(projectile.velocity.X) < 15f && Math.Abs(projectile.velocity.Y) < 15f)
                {
                    projectile.velocity *= 1.1f;
                }
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
            if (apShotFromLauncherID == 758)
            {
                if (projectile.ai[1] == 200f)
                {
                    projectile.Kill();
                }
                else
                {
                    projectile.ai[1] += 1f;
                }
            }
            if (apShotFromLauncherID == 760)
            {
                if (projectile.ai[1] < 3f)
                {
                    projectile.velocity *= 0.98f;
                }
                if (projectile.ai[1] >= 3f && projectile.alpha < 150)
                {
                    projectile.alpha++;
                }
            }
            if (apShotFromLauncherID == 1946)
            {
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
                AmmoHelper.chaseEnemy(projectile.identity, projectile.type);
            }
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
                Main.PlaySound(50, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, string.Format("Sounds/Custom/blackHole")), 0.8f, 0f);
            }
            AmmoHelper.explodeRocket(apShotFromLauncherID, projectile.identity, projectile.owner, default(Color), false, false);
        }
    }
}
