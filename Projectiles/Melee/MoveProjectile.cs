using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Melee
{
    class MoveProjectile : ModProjectile
    {
        public const int specialProjFrames = 5;
        
        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.aiStyle = -1;
            projectile.timeLeft = 60;

            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.penetrate = -1;
        }

        public float FrameCheck
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public int SlashLogic
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        Vector2? preDashVelocity;
        bool firstFrame = true;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            {
                float dashFrameDuration = 6;
                float dashSpeed = 32f;
                int freezeFrame = 2;
                bool dashing = player.AIDashSlash(projectile, dashFrameDuration, dashSpeed, freezeFrame, ref preDashVelocity);

                if (dashing)
                {
                    Dust.NewDust(player.Center - new Vector2(6, 6), 4, 4, 20);

                    // Coloured line trail
                    Vector2 dashStep = player.position - player.oldPosition;
                    for (int i = 0; i < 8; i++)
                    {
                        Dust d = Main.dust[Dust.NewDust(player.Center - (dashStep / 8) * i,
                            0, 0, 181, dashStep.X / 32, dashStep.Y / 32, 0, default(Color), 1.3f)];
                        d.noGravity = true;
                        d.velocity *= 0.1f;
                    }
                }

                // Calculate ending position dust
                if (firstFrame)
                {
                    firstFrame = false;

                    Vector2 endPosition = player.position;
                    Vector2 dashVector = projectile.velocity * dashSpeed;
                    for (int i = 0; i < dashFrameDuration * 2; i++)
                    {
                        Vector2 move = Collision.TileCollision(
                            endPosition, dashVector / 2,
                            player.width, player.height,
                            false, false, (int)player.gravDir);
                        if (move == Vector2.Zero) break;
                        endPosition += move;
                    }

                    // dash dust from the total distance over the duration
                    Vector2 totalDistanceStep =
                        (endPosition + new Vector2(player.width / 2, player.height / 2)
                        - player.Center) / dashFrameDuration;
                    for (int i = 0; i < dashFrameDuration; i++)
                    {
                        Vector2 pos = player.Center + (totalDistanceStep * i) - new Vector2(4, 4);
                        for (int j = 0; j < 5; j++)
                        {
                            pos += totalDistanceStep * (j / 5f);
                            Dust d = Main.dust[Dust.NewDust(pos, 0, 0,
                                175, projectile.velocity.X, projectile.velocity.Y,
                                0, Color.White, 1f)];
                            d.noGravity = true;
                            d.velocity *= 0.05f;
                        }
                    }
                }
            }

            projectile.damage = 0;
            FrameCheck += 1f; // Framerate
        }
    }
}
