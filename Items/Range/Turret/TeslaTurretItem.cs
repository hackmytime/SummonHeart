using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items.Range.Power;
using SummonHeart.Tiles.Range;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Turret
{
    public class TeslaTurretItem : TurretItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("4级科技造物·闪电炮塔");
            Tooltip.SetDefault("1W福特");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 0;
        }

        protected override int PickTile()
        {
            return ModContent.TileType<TeslaTurretTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(1552, 4);
            modRecipe.AddIngredient(3261, 4);
            modRecipe.AddIngredient(ModContent.ItemType<Power1>(), 1);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
