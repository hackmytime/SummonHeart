using SummonHeart.Extensions.TurretSystem;
using SummonHeart.NPCs.Range;
using Terraria.ModLoader;

namespace SummonHeart.Tiles.Range
{
    public class TETeslaTurretTile : TurretTileEntity
    {
        public override int GetHeadType()
        {
            return ModContent.NPCType<TeslaTurretHead>();
        }

        public override int GetTileType()
        {
            return ModContent.TileType<TeslaTurretTile>();
        }
    }
}
