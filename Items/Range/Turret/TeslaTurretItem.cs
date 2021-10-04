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
            DisplayName.SetDefault("原初炮塔");
            Tooltip.SetDefault("0号原型机，无法获取");
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
    }
}