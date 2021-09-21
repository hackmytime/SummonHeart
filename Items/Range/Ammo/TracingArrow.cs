using Terraria;
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
            recipe.AddIngredient(ItemID.WoodenArrow, 200);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.AddIngredient(mod.GetItem("VUnit"), 1);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod); recipe.AddIngredient(ItemID.WoodenArrow, 9999); recipe.AddIngredient(mod.GetItem("Loot5"), 1); recipe.SetResult(this, 9999); recipe.AddRecipe();
            recipe = new ModRecipe(mod); recipe.AddIngredient(ItemID.WoodenArrow, 999); recipe.AddIngredient(mod.GetItem("Loot4"), 1); recipe.SetResult(this, 999); recipe.AddRecipe();
            recipe = new ModRecipe(mod); recipe.AddIngredient(ItemID.WoodenArrow, 99); recipe.AddIngredient(mod.GetItem("Loot3"), 1); recipe.SetResult(this, 99); recipe.AddRecipe();
            recipe = new ModRecipe(mod); recipe.AddIngredient(ItemID.WoodenArrow, 9); recipe.AddIngredient(mod.GetItem("Loot2"), 1); recipe.SetResult(this, 9); recipe.AddRecipe();
            recipe = new ModRecipe(mod); recipe.AddIngredient(ItemID.WoodenArrow, 1); recipe.AddIngredient(mod.GetItem("Loot1"), 1); recipe.SetResult(this, 1); recipe.AddRecipe();
        }
    }
}
