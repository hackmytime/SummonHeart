using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Ammo.Bullet
{
    internal class BlackholeBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("3级科技造物·黑洞弹");
            Tooltip.SetDefault("爆炸生成一个微型黑洞");
        }

        public override void SetDefaults()
        {
            item.damage = 1;
            item.ranged = true;
            item.width = 12;
            item.height = 16;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2f;
            item.value = 15;
            item.rare = 7;
            item.shootSpeed = 5f;
            item.shoot = mod.ProjectileType("BlackholeBulletPro");
            item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(548, 15);
            modRecipe.AddIngredient(521, 15);
            modRecipe.AddIngredient(774, 10);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 10);
            modRecipe.AddRecipe();
        }
    }
}
