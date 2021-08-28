using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Magic
{
    public class MagicSword : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.width = 38;
            projectile.height = 38;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 500;
            projectile.damage = 100;
            projectile.aiStyle = 0;
            projectile.scale = 0.6f;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        
        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer mp = player.SH();
			float B3 = projectile.Center.X;
			float C3 = projectile.Center.Y;
			float D2 = 500f;
			bool flag18 = false;
			for (int A3 = 0; A3 < 200; A3++)
			{
				if (Main.npc[A3].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[A3].Center, 1, 1))
				{
					float G2 = Main.npc[A3].position.X + (float)(Main.npc[A3].width / 2);
					float A4 = Main.npc[A3].position.Y + (float)(Main.npc[A3].height / 2);
					float B4 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - G2) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - A4);
					if (B4 < D2)
					{
						D2 = B4;
						B3 = G2;
						C3 = A4;
						flag18 = true;
					}
				}
			}
			if (flag18)
			{
				float num2 = 8f;
				Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float E2 = B3 - vector3.X;
				float F2 = C3 - vector3.Y;
				float C4 = (float)Math.Sqrt((double)(E2 * E2 + F2 * F2));
				C4 = num2 / C4;
				E2 *= C4;
				F2 *= C4;
				projectile.velocity.X = (projectile.velocity.X * 10f + E2) / 10.5f;
				projectile.velocity.Y = (projectile.velocity.Y * 10f + F2) / 10.5f;
			}
		}
    }
}
