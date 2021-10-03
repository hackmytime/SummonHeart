using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    public class VBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("2级科技造物·穿透弹");
            Tooltip.SetDefault("穿透+8");
        }

        public override void SetDefaults()
        {
            item.damage = 9;
            item.ranged = true;
            item.width = 8;
            item.height = 12;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2f;
            item.value = 50;
            item.rare = 3;
            item.shoot = mod.ProjectileType("VBulletPro");
            item.shootSpeed = 5f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 1);
            recipe.AddIngredient(mod.GetItem("VUnit"), 1);
            recipe.SetResult(this, 500);
            recipe.AddRecipe();
        }
    }
}
