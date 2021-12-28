using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.XiuXian
{
    public class XiuLianProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Shell");
        }

        public override void SetDefaults()
        {
            projectile.width = 75;
            projectile.height = 98;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 0.75f;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (player.dead)
            {
                modPlayer.XiuLian = false;
            }

            if (!modPlayer.XiuLian)
            {
                projectile.Kill();
                return;
            }

            projectile.position.X = Main.player[projectile.owner].Center.X - projectile.width / 2;
            projectile.position.Y = Main.player[projectile.owner].Center.Y - projectile.height / 2 - 21;
        }
    }
}