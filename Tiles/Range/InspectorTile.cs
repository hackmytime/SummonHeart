using System;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Range.Tools;
using SummonHeart.ui;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SummonHeart.Tiles.Range
{
    public class InspectorTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            soundType = 2;
            soundStyle = 14;
            dustType = -1;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            Main.tileWaterDeath[Type] = true;
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.addTile(Type);
            animationFrameHeight = 54;
            ModTranslation name = CreateMapEntryName(null);
            AddMapEntry(new Color(129, 69, 120), name);
            disableSmartCursor = true;
        }

        public override bool NewRightClick(int i, int j)
        {
            Main.playerInventory = true;
            ModUIHandler.dinoLoader.ShowUI(new Vector2(i * 16, j * 16));
            return true;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 9)
            {
                frameCounter = 0;
                frame++;
                frame %= 25;
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (!Main.dedServ)
            {
                Vector2 pos = new Vector2((float)(i * 16), (float)(j * 16));
                Color defaultColor = new Color(47, 54, 71);
                float defaultScale = 1.0289474f;
                float defaultSpeedX = 6.063158f;
                float defaultSpeedY = 0.002263f;
                for (int e = 0; e < 5; e++)
                {
                    Dust.NewDustDirect(pos, 0, 0, 5, defaultSpeedX, defaultSpeedY, 0, defaultColor, defaultScale);
                }
                Color sparkColor = new Color(234, 255, 0);
                float sparkScale = 0.4389474f;
                Dust.NewDustDirect(pos, 0, 0, 6, 1.3631581f, -1.105263f, 0, sparkColor, sparkScale);
                Dust.NewDustDirect(pos, 0, 0, 6, -2.363158f, -1.805263f, 0, sparkColor, sparkScale);
                Dust.NewDustDirect(pos, 0, 0, 6, 0.4631581f, -1.405263f, 0, sparkColor, sparkScale);
                Dust.NewDustDirect(pos, 0, 0, 6, 1.663158f, -1.205263f, 0, sparkColor, sparkScale);
                Dust.NewDustDirect(pos, 0, 0, 6, 2.363158f, 1.305263f, 0, sparkColor, sparkScale);
                Dust.NewDustDirect(pos, 0, 0, 226, -2.363158f, -2.105263f, 0, sparkColor, sparkScale);
                Dust.NewDustDirect(pos, 0, 0, 6, 1.9631581f, -1.405263f, 0, sparkColor, sparkScale);
                Dust dust = Dust.NewDustDirect(pos, 0, 0, 226, -1.4631581f, -1.005263f, 0, sparkColor, 0.4289474f);
                dust.noGravity = true;
                dust.noLight = false;
                dust.fadeIn = 0.2131579f;
                Dust dust2 = Dust.NewDustDirect(pos, 0, 0, 226, 1.5631582f, -2.505263f, 0, sparkColor, 0.5189474f);
                dust2.noGravity = true;
                dust2.noLight = false;
                dust2.fadeIn = 0.1131579f;
                Dust dust3 = Dust.NewDustDirect(pos, 0, 0, 226, -0.8631581f, -1.955263f, 0, sparkColor, 0.4289474f);
                dust3.noGravity = true;
                dust3.noLight = false;
                Dust dust4 = Dust.NewDustDirect(pos, 0, 0, 6, 0.2631581f, 1.505263f, 0, sparkColor, 0.4289474f);
                dust4.noGravity = false;
                dust4.noLight = false;
                dust4.fadeIn = 0.9831579f;
                Dust dust5 = Dust.NewDustDirect(pos, 0, 0, 6, -1.3631581f, 2.105263f, 0, sparkColor, 0.4189474f);
                dust5.noGravity = true;
                dust5.noLight = false;
                dust5.fadeIn = 0.86731577f;
            }
            Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Inspector>(), 1, false, 0, false, false);
        }
    }
}
