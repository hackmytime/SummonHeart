using SummonHeart.Extensions.TurretSystem;
using SummonHeart.NPCs.Range;
using Terraria.ModLoader;

namespace SummonHeart.Tiles.Range
{
    public class TELightTurretTile4 : TurretTileEntity
    {
        public override int GetHeadType()
        {
            return ModContent.NPCType<LightTurretHead4>();
        }

        public override int GetTileType()
        {
            return ModContent.TileType<LightTurretTile4>();
        }
    }
}
