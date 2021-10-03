using SummonHeart.Projectiles.Range.Bullet;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    internal class LowBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3级科技造物·劣质狙击弹");
            Tooltip.SetDefault("18基础伤害+36穿透+追踪+持续时间6秒");
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
            item.value = Item.sellPrice(1, 0, 0, 0);
            item.rare = -12;
            item.shoot = ModContent.ProjectileType<LowBulletPro>();
            item.shootSpeed = 5f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 1);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.AddIngredient(mod.GetItem("VUnit"), 1);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
