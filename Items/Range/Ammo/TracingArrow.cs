﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SummonHeart.Projectiles.Range.Arrows;

namespace SummonHeart.Items.Range.Ammo
{
    public class TracingArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("2级科技造物·追踪穿透箭矢" +
                "\n穿墙追踪，穿透+8，无视敌人无敌帧" +
                "\n科技造物，造价高昂，品质保证");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 8;
            item.height = 8;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 1.5f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = -12;
            item.shoot = ModContent.ProjectileType<TracingArrowPro>();
            item.shootSpeed = 12f;
            item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 1);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.AddIngredient(mod.GetItem("VUnit"), 1);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }
    }
}
