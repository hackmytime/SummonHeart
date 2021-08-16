using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using System;
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
                //int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.GoldCoin);
                int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, MyDustId.BlueMagic);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                Vector2 vel = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                vel.Normalize();
                Main.dust[dust].velocity = vel * Main.rand.Next(50, 100) * 0.04f;
                Main.dust[dust].position = projectile.Center - vel * 34f;
            }
            return base.PreAI(projectile);
        }

        public override void AI(Projectile projectile)
        {
			if (projectile.type == 116)
			{
				Player player = Main.player[Main.myPlayer];
				SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
				int range = (int)(modPlayer.magicSwordBlood / 16.7 + 200);
				if (range > 1000)
					range = 1000;
				float B3 = projectile.Center.X;
				float C3 = projectile.Center.Y;
				float D2 = range;
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
}
