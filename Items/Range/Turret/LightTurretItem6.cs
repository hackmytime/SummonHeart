﻿using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items.Range.AmmoSkill;
using SummonHeart.Items.Range.Armor;
using SummonHeart.Items.Range.Mate;
using SummonHeart.Items.Range.Power;
using SummonHeart.Tiles.Range;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Turret
{
    public class LightTurretItem6 : TurretItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("6级科技造物·高级闪电炮塔");
            Tooltip.SetDefault("放置后无法破坏" +
                "\n护盾值：2500W" +
                "\n攻击速度：1秒5次" +
                "\n攻击范围：45码" +
                "\n每次攻击造成200000点伤害");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 0;
        }

        protected override int PickTile()
        {
            return ModContent.TileType<LightTurretTile6>();
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<LightGenerator3>(), 5);
            modRecipe.AddIngredient(ModContent.ItemType<CoreComputer>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<Power6>(), 50);
            modRecipe.AddIngredient(ModContent.ItemType<PowerArmor6>(), 5);
            modRecipe.AddIngredient(ModContent.ItemType<MetalUnit>(), 500);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
