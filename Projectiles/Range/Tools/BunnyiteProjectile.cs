using System;
using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace SummonHeart.Projectiles.Range.Tools
{
    internal class BunnyiteProjectile : ExplosiveProjectile
    {
        protected override string explodeSoundsLoc
        {
            get
            {
                return "n/a";
            }
        }

        protected override string goreFileLoc
        {
            get
            {
                return "Gores/Explosives/bunnyite_gore";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bunnyite");
        }

        public override void SafeSetDefaults()
        {
            IgnoreTrinkets = true;
            projectile.tileCollide = true;
            projectile.width = 10;
            projectile.height = 32;
            projectile.aiStyle = 16;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 80;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item14, (int)projectile.Center.X, (int)projectile.Center.Y);
            Explosion();
            CreateDust(projectile.Center, 50);
        }

        public override void Explosion()
        {
            Vector2 position = projectile.Center;
            int bunnies = 200;
            for (int x = 0; x < bunnies; x++)
            {
                int v = NPC.NewNPC((int)position.X + Main.rand.Next(1000) - 500, (int)position.Y, 46, 0, 0f, 0f, 0f, 0f, 255); 
                if(Main.netMode != 0)
                    Main.npc[v].netUpdate = true;
            }
        }

        private void CreateDust(Vector2 position, int amount)
        {
            for (int i = 0; i <= amount; i++)
            {
                if (Main.rand.NextFloat() < 1f)
                {
                    Vector2 updatedPosition = new Vector2(position.X - 400f, position.Y - 50f);
                    Dust dust = Main.dust[Dust.NewDust(updatedPosition, 800, 100, 112, 0f, 0f, 0, new Color(255, 0, 0), 1.447368f)];
                    if (Vector2.Distance(dust.position, projectile.Center) > 400f)
                    {
                        dust.active = false;
                    }
                    dust.shader = GameShaders.Armor.GetSecondaryShader(36, Main.LocalPlayer);
                    dust.fadeIn = 1.144737f;
                }
            }
        }
    }
}
