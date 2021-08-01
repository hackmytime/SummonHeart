using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.Items
{
    // Token: 0x02000006 RID: 6
    public class AllItemsMenu : GlobalItem
    {
        // Token: 0x06000013 RID: 19 RVA: 0x00002274 File Offset: 0x00000474
        public AllItemsMenu()
        {
            singleSlotArray = new Item[1];
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002288 File Offset: 0x00000488
        internal void DrawUpdateExtraAccessories(SpriteBatch spriteBatch)
        {
            if (Main.EquipPage == 0)
            {
                Point value = new Point(Main.mouseX, Main.mouseY);
                Rectangle r = new Rectangle(0, 0, (int)(Main.inventoryBackTexture.Width * Main.inventoryScale), (int)(Main.inventoryBackTexture.Height * Main.inventoryScale));

                var mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
                for (int i = 0; i < SummonHeartPlayer.MaxExtraAccessories; i++)
                {
                    Main.inventoryScale = 0.85f;
                    Item accItem = mp.ExtraAccessories[i];
                    int x = i % 10;
                    int y = i / 10;
                    float vx = Main.inventoryBack9Texture.Width * 0.918f * x;
                    float vy = Main.inventoryBack9Texture.Height * 0.918f * y;
                    r.X = (int)(SummonHeartConfig.Instance.accX + vx);
                    r.Y = (int)(SummonHeartConfig.Instance.accY + vy);
                    Vector2 pos = new Vector2(r.X, r.Y);
                    if (r.Contains(value))
                    {
                        Main.LocalPlayer.mouseInterface = true;
                        Main.armorHide = true;
                        singleSlotArray[0] = accItem;
                        if (Main.mouseItem.type == 0 || Main.LocalPlayer.HasItemInAcc(Main.mouseItem.type) == -1)
                        {
                            ItemSlot.Handle(singleSlotArray, 10, 0);
                        }
                        accItem = singleSlotArray[0];
                    }
                    singleSlotArray[0] = accItem;
                    ItemSlot.Draw(spriteBatch, singleSlotArray, 10, 0, pos, default);
                    accItem = singleSlotArray[0];
                    mp.ExtraAccessories[i] = accItem;
                }
            }
        }

        // Token: 0x04000008 RID: 8
        internal static Item[] singleSlotArray;
    }
}
