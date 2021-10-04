using SummonHeart.Extensions.TurretSystem;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
			if (i < 0 || j < 0 || i >= Main.maxTilesX || j >= Main.maxTilesY)
			{
				return false;
			}
			Tile tile = Main.tile[i, j];
			Tile tile2 = null;
			if (tile == null)
			{
				return false;
			}
			if (!tile.active())
			{
				return false;
			}
			if (j >= 1)
			{
				tile2 = Main.tile[i, j - 1];
			}
			if (tile2 != null && tile2.active())
			{
				ModTile modTile = TileLoader.GetTile(tile2.type);
				if (modTile is TurretTile)
                {
					return false;
                }
			}
			return base.CanKillTile(i, j, type, ref blockDamaged);
        }

        public override void RandomUpdate(int i, int j, int type)
        {
			if (Main.tile[i, j].inActive())
			{
				return;
			}
			if ((type == 117 || type == 164) && (double)j > Main.rockLayer && WorldGen.genRand.Next(110) == 0)
			{
				int num = WorldGen.genRand.Next(4);
				int num2 = 0;
				int num3 = 0;
				if (num == 0)
				{
					num2 = -1;
				}
				else if (num == 1)
				{
					num2 = 1;
				}
				else if (num == 0)
				{
					num3 = -1;
				}
				else
				{
					num3 = 1;
				}
				if (!Main.tile[i + num2, j + num3].active())
				{
					int num4 = 0;
					int num5 = 6;
					for (int k = i - num5; k <= i + num5; k++)
					{
						for (int l = j - num5; l <= j + num5; l++)
						{
							if (Main.tile[k, l].active() && Main.tile[k, l].type == 129)
							{
								num4++;
							}
						}
					}
					if (num4 < 2)
					{
						WorldGen.PlaceTile(i + num2, j + num3, 129, true, false, -1, 0);
						NetMessage.SendTileSquare(-1, i + num2, j + num3, 1, TileChangeType.None);
					}
				}
			}
			if ((double)j > (Main.worldSurface + Main.rockLayer) / 2.0)
			{
				if (type == 60 && WorldGen.genRand.Next(300) == 0)
				{
					int num6 = i + WorldGen.genRand.Next(-10, 11);
					int num7 = j + WorldGen.genRand.Next(-10, 11);
					if (WorldGen.InWorld(num6, num7, 2) && Main.tile[num6, num7].active() && Main.tile[num6, num7].type == 59 && (!Main.tile[num6, num7 - 1].active() || (Main.tile[num6, num7 - 1].type != 5 && Main.tile[num6, num7 - 1].type != 236 && Main.tile[num6, num7 - 1].type != 238)) && WorldGen.Chlorophyte(num6, num7))
					{
						Main.tile[num6, num7].type = 211;
						WorldGen.SquareTileFrame(num6, num7, true);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, num6, num7, 1, TileChangeType.None);
						}
					}
				}
				if (type == 211 && WorldGen.genRand.Next(3) != 0)
				{
					int num8 = i;
					int num9 = j;
					int num10 = WorldGen.genRand.Next(4);
					if (num10 == 0)
					{
						num8++;
					}
					if (num10 == 1)
					{
						num8--;
					}
					if (num10 == 2)
					{
						num9++;
					}
					if (num10 == 3)
					{
						num9--;
					}
					if (WorldGen.InWorld(num8, num9, 2) && Main.tile[num8, num9].active() && (Main.tile[num8, num9].type == 59 || Main.tile[num8, num9].type == 60) && WorldGen.Chlorophyte(num8, num9))
					{
						Main.tile[num8, num9].type = 211;
						WorldGen.SquareTileFrame(num8, num9, true);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, num8, num9, 1, TileChangeType.None);
						}
					}
					bool flag = true;
					while (flag)
					{
						flag = false;
						num8 = i + Main.rand.Next(-5, 6);
						num9 = j + Main.rand.Next(-5, 6);
						if (WorldGen.InWorld(num8, num9, 2) && Main.tile[num8, num9].active())
						{
							if (Main.tile[num8, num9].type == 23 || Main.tile[num8, num9].type == 199 || Main.tile[num8, num9].type == 2 || Main.tile[num8, num9].type == 109)
							{
								Main.tile[num8, num9].type = 60;
								WorldGen.SquareTileFrame(num8, num9, true);
								if (Main.netMode == 2)
								{
									NetMessage.SendTileSquare(-1, num8, num9, 1, TileChangeType.None);
								}
								flag = true;
							}
							else if (Main.tile[num8, num9].type == 0)
							{
								Main.tile[num8, num9].type = 59;
								WorldGen.SquareTileFrame(num8, num9, true);
								if (Main.netMode == 2)
								{
									NetMessage.SendTileSquare(-1, num8, num9, 1, TileChangeType.None);
								}
								flag = true;
							}
						}
					}
				}
			}
			if (NPC.downedPlantBoss && WorldGen.genRand.Next(2) != 0)
			{
				return;
			}
		}
    }
}