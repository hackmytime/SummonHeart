using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    public class DarkBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("2级科技造物·诅咒弹");
            Tooltip.SetDefault("给击中的敌人施加诅咒，诅咒会伤害周围的敌人");
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
            item.shoot = mod.ProjectileType("DarkBulletPro");
            item.shootSpeed = 5f;
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(331, 1);
            modRecipe.AddIngredient(1432, 100);
            modRecipe.SetResult(this, 100);
            modRecipe.AddTile(134);
            modRecipe.AddRecipe();
        }
    }
}
