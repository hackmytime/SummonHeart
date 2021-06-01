using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Pets
{
    public class llPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = 26;
            aiType = ProjectileID.BabyHornet;
            projectile.penetrate = -1;
            projectile.netImportant = true;
            projectile.timeLeft *= 5;
            projectile.scale = 0.6f;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.manualDirectionChange = true; 
        }

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.petFlagDD2Gato = false; // Relic from aiType
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            if (player.dead)
            {
                modPlayer.llPet = false;
            }
            if (modPlayer.llPet)
            {
                projectile.timeLeft = 2;
            }
            Lighting.AddLight((int)((double)projectile.position.X + (double)(projectile.width / 2)) / 16, (int)((double)projectile.position.Y + (double)(projectile.height / 2)) / 16, 0.8f, 0.95f, 1f);
        }
    }
}