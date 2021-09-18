using System;
using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Bullet
{
    internal class BlackHole : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Hole");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 1;
            projectile.timeLeft = 300;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            Main.projFrames[projectile.type] = 5;
        }

        public override void AI()
        {
            Projectile projectile = base.projectile;
            int frame;
            if (base.projectile.frame != 4)
            {
                Projectile projectile2 = base.projectile;
                int i = projectile2.frame + 1;
                projectile2.frame = i;
                frame = i;
            }
            else
            {
                frame = 0;
            }
            projectile.frame = frame;
            if (base.projectile.timeLeft <= 60)
            {
                base.projectile.alpha += 4;
            }
            AmmoHelper.createDustCircle(base.projectile.Center, 199, 128f, true, true, 64, new Color(255, 255, 255), 8, 8, new Vector2(0f, 0f), 0.0, 0);
            Vector2 center = base.projectile.Center;
            int dustType = 211;
            float radius = 16f;
            bool noGravity = true;
            bool newDustPerfect = true;
            int count = 16;
            Vector2 velocity = Vector2.Zero;
            AmmoHelper.createDustCircle(center, dustType, radius, noGravity, newDustPerfect, count, new Color(255, 0, 251), 8, 8, velocity, 0.0, 0);
            base.projectile.velocity = Vector2.Zero;
            double maxRange = 128.0;
            foreach (NPC npc in Main.npc)
            {
                double x = base.projectile.Center.X - npc.Center.X;
                double y = base.projectile.Center.Y - npc.Center.Y;
                if (Vector2.Distance(base.projectile.Center, npc.Center) < maxRange && npc.active && !npc.boss)
                {
                    Vector2 vel = new Vector2((float)x, (float)y) * 0.04f;
                    npc.velocity = vel;
                    if (Main.netMode == 1)
                    {
                        ModPacket packet = mod.GetPacket(256);
                        packet.Write(6);
                        packet.Write(npc.whoAmI);
                        packet.WriteVector2(vel);
                        packet.Send(-1, -1);
                    }
                }
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return new bool?(false);
        }
    }
}
