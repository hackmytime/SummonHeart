using SummonHeart.Projectiles.Range.Bullet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    internal class MaxBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("4级科技造物·究极狙击弹");
            Tooltip.SetDefault("20基础伤害+无限穿透+追踪+持续时间6秒");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.ranged = true;
            item.width = 12;
            item.height = 16;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 1.5f;
            item.value = Item.sellPrice(10, 0, 0, 0);
            item.rare = -12;
            item.shoot = ModContent.ProjectileType<MaxBulletPro>();
            item.shootSpeed = 5f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 6);
            recipe.AddIngredient(mod.GetItem("VUnit"), 6);
            recipe.AddIngredient(mod.GetItem("Power4"), 1);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.SetResult(this, 6);
            recipe.AddRecipe();
        }
    }
}
