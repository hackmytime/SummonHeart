using SummonHeart.Extensions.TurretSystem;
using SummonHeart.NPCs.Range;
using Terraria.ModLoader;

namespace SummonHeart.Tiles.Range
{
    public class TELightTurretTile1 : TurretTileEntity
    {
        public override int GetHeadType()
        {
            return ModContent.NPCType<LightTurretHead1>();
        }

        public override int GetTileType()
        {
            return ModContent.TileType<LightTurretTile1>();
        }
    }
}
