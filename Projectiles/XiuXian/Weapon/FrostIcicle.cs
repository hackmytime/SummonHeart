﻿using System;
using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using SummonHeart.XiuXianModule.Entities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.XiuXian.Weapon
{
    public class FrostIcicle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Icicle");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 14;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 2;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            RPGPlayer modPlayer = player.GetModPlayer<RPGPlayer>();

            projectile.timeLeft++;
            projectile.netUpdate = true;

            if (player.whoAmI == Main.myPlayer && player.dead)
            {
                modPlayer.onIceAttack = false;
            }

            /*if (player.whoAmI == Main.myPlayer && ! modPlayer.onIceAttack)
            {
                projectile.Kill();
                return;
            }*/

            if (projectile.owner == Main.myPlayer)
            {
                //rotation mumbo jumbo
                float distanceFromPlayer = projectile.ai[0];
                Vector2 center = player.Center;
                center.Y -=  Main.screenHeight / 4;
                projectile.position = center + new Vector2(distanceFromPlayer, 0f).RotatedBy(projectile.ai[1]);
                projectile.position.X -= projectile.width / 2;
                projectile.position.Y -= projectile.height / 2;

                float rotation = (float)Math.PI / 120;
                projectile.ai[1] -= rotation;
                if (projectile.ai[1] > (float)Math.PI)
                {
                    projectile.ai[1] -= 2f * (float)Math.PI;
                    projectile.netUpdate = true;
                }

                projectile.rotation = (Main.MouseWorld - projectile.Center).ToRotation() - 5;
            }

            if (Main.netMode == NetmodeID.Server)
                projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].GetModPlayer<RPGPlayer>().IcicleCount--;
        }
    }
}