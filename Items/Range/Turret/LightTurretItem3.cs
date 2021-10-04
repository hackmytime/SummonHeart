using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items.Range.AmmoSkill;
using SummonHeart.Items.Range.Armor;
using SummonHeart.Items.Range.Mate;
using SummonHeart.Items.Range.Power;
using SummonHeart.Tiles.Range;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Turret
{
    public class LightTurretItem3 : TurretItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("5级科技造物·劣质中级闪电炮塔");
            Tooltip.SetDefault("放置后无法破坏" +
                "\n护盾值：150W" +
                "\n攻击速度：1秒3次" +
                "\n攻击范围：30码" +
                "\n每次攻击造成2000点伤害");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 0;
        }

        protected override int PickTile()
        {
            return ModContent.TileType<LightTurretTile3>();
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<LightGenerator2>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<CoreComputer>(), 1);
            modRecipe.AddIngredient(ModContent.ItemType<Power3>(), 30);
            modRecipe.AddIngredient(ModContent.ItemType<PowerArmor3>(), 3);
            modRecipe.AddIngredient(ModContent.ItemType<MetalUnit>(), 300);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
