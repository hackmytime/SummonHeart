﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.XiuXianModule.Weapon.Power
{
    public class VortexMagnetRitual : LinliDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex Ritual");
            Tooltip.SetDefault("'Power surges in your hand'");
        }


        public override void SafeSetDefaults()
        {
            item.damage = 280;
            item.magic = true;
            item.useTime = 16;
            item.useAnimation = 16;
            item.knockBack = 4f;
            item.mana = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<VortexRitualProj>();
            item.shootSpeed = 12f;
            item.channel = true;

            item.width = 28;
            item.height = 30;
            item.value = Item.sellPrice(0, 12);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item21;
            linliCost = 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //initial spawn
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VortexRitualProj>()] <= 0)
            {
                Vector2 mouse = Main.MouseWorld;
                Projectile.NewProjectile(mouse, Vector2.Zero, ModContent.ProjectileType<VortexRitualProj>(), damage, knockBack, player.whoAmI, 0, 300);

                //some funny dust
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MagnetSphere);
            recipe.AddIngredient(ItemID.FragmentVortex, 35);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
