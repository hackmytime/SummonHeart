﻿using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items.Range.AmmoSkill;
using SummonHeart.Items.Range.Armor;
using SummonHeart.Items.Range.Mate;
using SummonHeart.Items.Range.Power;
using SummonHeart.Tiles.Range;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Turret
{
    public class LightTurretItem1 : TurretItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("4级科技造物·劣质初级闪电炮塔");
            Tooltip.SetDefault("放置后无法破坏" +
                "\n护盾值：10W" +
                "\n攻击速度：1秒2次" +
                "\n攻击范围：20码" +
                "\n每次攻击造成100点伤害");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 0;
        }

        protected override int PickTile()
        {
            return ModContent.TileType<LightTurretTile1>();
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<LightGenerator1>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<CoreComputer>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<Power1>(), 10);
            modRecipe.AddIngredient(ModContent.ItemType<PowerArmor1>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<MetalUnit>(), 100);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
