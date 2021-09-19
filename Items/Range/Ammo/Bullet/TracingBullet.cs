using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SummonHeart.Projectiles.Range.Bullet;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    public class TracingBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("2级科技造物·追踪弹" +
                "\n自动索敌，弹无虚发" +
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
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = -12;
            item.shoot = ModContent.ProjectileType<TracingBulletPro>();
            item.shootSpeed = 5f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusketBall, 500);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.SetResult(this, 500);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusketBall, 9999);
            recipe.AddIngredient(mod.GetItem("Loot5"), 1);
            recipe.SetResult(this, 9999);
            recipe.AddRecipe();
        }
    }
}
