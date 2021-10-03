using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SummonHeart.Projectiles.Range.Bullet;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    public class VTracingBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3级科技造物·追踪穿透弹" +
                "\n自动索敌，弹无虚发" +
                "\n穿透+8" +
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
            item.shoot = ModContent.ProjectileType<VTracingBulletPro>();
            item.shootSpeed = 5f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 1);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.AddIngredient(mod.GetItem("VUnit"), 1);
            recipe.SetResult(this, 500);
            recipe.AddRecipe();
        }
    }
}
