using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.Extensions.TurretSystem
{
    public abstract class TurretTileEntity : ModTileEntity
    {
        public abstract int GetHeadType();

        public abstract int GetTileType();

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 5, 0);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i, j);
        }

        public override void Update()
        {
            if (storedHead == null)
            {
                xPos = Position.X * 16 + 24;
                yPos = Position.Y * 16 + 48;
                if (Main.tile[Position.X, Position.Y].frameX == 0)
                {
                    storedDirection = 1;
                }
                else
                {
                    storedDirection = 0;
                }
                SpawnHead();
                return;
            }
            TurretHead head = (TurretHead)storedHead.modNPC;
            isAlive = head.isAlive;
        }

        private void SpawnHead()
        {
            storedHead = Main.npc[NPC.NewNPC(xPos, yPos, GetHeadType(), 0, 0f, 0f, 0f, 0f, 255)];
            TurretHead head = (TurretHead)storedHead.modNPC;
            head.Direction = storedDirection;
            head.InitRotation();
            if (!isAlive)
            {
                head.Deactivate();
            }
            NetMessage.SendData(23, -1, -1, null, storedHead.whoAmI, 0f, 0f, 0f, 0, 0, 0);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == GetTileType() && tile.frameY == 0;
        }

        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound();
            tagCompound["isAlive"] = isAlive;
            return tagCompound;
        }

        public override void Load(TagCompound tag)
        {
            isAlive = tag.GetBool("isAlive");
        }

        internal NPC storedHead;

        internal int storedDirection;

        internal int xPos;

        internal int yPos;

        internal bool isAlive = true;
    }
}
