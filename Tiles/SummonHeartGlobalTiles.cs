using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SummonHeart.Tiles
{
    public class SummonHeartGlobalTiles : GlobalTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[TileID.DesertFossil] = true;
            Main.tileSpelunker[TileID.Hellstone] = true;
            Main.tileSpelunker[TileID.Slush] = true;
            Main.tileSpelunker[TileID.Silt] = true;
        }
    }
}