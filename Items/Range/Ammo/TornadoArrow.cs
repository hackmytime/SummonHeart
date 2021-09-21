using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SummonHeart.Projectiles.Range.Arrows;

namespace SummonHeart.Items.Range.Ammo
{
    public class TornadoArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("4级科技造物·风暴狙击箭" +
                "\n基础攻击20，命中时爆炸根据冷热原理生成巨型龙卷风，龙卷风吸收范围内一切npc物块和玩家" +
                "\n每秒钟造成10次暴风伤害，持续时间10秒，最大100次伤害" +
                "\n科技造物，造价高昂，品质保证");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.ranged = true;
            item.width = 8;
            item.height = 8;
            item.maxStack = 6;
            item.consumable = true;
            item.knockBack = 1.5f;
            item.value = Item.sellPrice(10, 0, 0, 0);
            item.rare = -12;
            item.shoot = ModContent.ProjectileType<TornadoArrowPro>();
            item.shootSpeed = 12f;
            item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("HotUnit"), 6);
            recipe.AddIngredient(mod.GetItem("IceUnit"), 6);
            recipe.AddIngredient(mod.GetItem("Power4"), 1);
            recipe.AddIngredient(mod.GetItem("TracingUnit"), 1);
            recipe.SetResult(this, 6);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot5"), 1);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot4"), 1);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
