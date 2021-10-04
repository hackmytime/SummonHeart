using SummonHeart.Extensions.TurretSystem;
using SummonHeart.NPCs.Range;
using Terraria.ModLoader;

namespace SummonHeart.Tiles.Range
{
    public class TELightTurretTile5 : TurretTileEntity
    {
        public override int GetHeadType()
        {
            return ModContent.NPCType<LightTurretHead5>();
        }

        public override int GetTileType()
        {
            return ModContent.TileType<LightTurretTile5>();
        }
    }
}
