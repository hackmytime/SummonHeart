using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SummonHeart.Extensions.TurretSystem
{
    public abstract class TurretTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                18
            };
            TileObjectData.newTile.Direction = (Terraria.Enums.TileObjectDirection)1;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(GetTE().Hook_AfterPlacement), -1, 0, true);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newAlternate.Direction = (Terraria.Enums.TileObjectDirection)2;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName(null);
            name.SetDefault(GetName());
            AddMapEntry(GetMapColor(), name);
            disableSmartCursor = true;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            GetTE().Kill(i, j);
            for (int index = 0; index < 200; index++)
            {
                NPC npc = Main.npc[index];
                if (npc.type == GetHead() && npc.getRect().Contains(i * 16 + 16, j * 16 + 16))
                {
                    if (((TurretHead)npc.modNPC).isAlive)
                    {
                        Item.NewItem(i * 16, j * 16, 48, 48, GetItem(), 1, false, 0, false, false);
                    }
                    npc.StrikeNPC(9999, 1f, 0, false, true, false);
                    npc.active = false;
                    return;
                }
            }
        }

        /*public override bool CanPlace(int i, int j)
        {
            bool flag = TurretWorld.CanPlaceTurret();
            if (!flag)
            {
                Main.NewText("Cannot place turret, world has maximum turrets placed.", byte.MaxValue, 50, 50, false);
            }
            return flag;
        }*/

        // Token: 0x06000077 RID: 119
        public abstract int GetHead();

        // Token: 0x06000078 RID: 120
        public abstract TurretTileEntity GetTE();

        // Token: 0x06000079 RID: 121
        public abstract int GetItem();

        // Token: 0x0600007A RID: 122
        public abstract string GetName();

        // Token: 0x0600007B RID: 123
        public abstract Color GetMapColor();
    }
}
