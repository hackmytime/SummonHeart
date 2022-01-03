using Terraria.ID;

namespace SummonHeart.XiuXianModule.Weapon.Power
{
    public class VortexBolt : LightningArc
    {
        public override string Texture => "Terraria/Projectile_466";

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.ranged = false;
            projectile.magic = true;

            projectile.usesIDStaticNPCImmunity = false;
            projectile.idStaticNPCHitCooldown = 0;

            projectile.timeLeft = 30 * (projectile.extraUpdates + 1);
        }
    }
}
