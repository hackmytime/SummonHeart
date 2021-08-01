using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using On.Terraria.UI;
using SummonHeart.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Utilities
{
    // Token: 0x02000005 RID: 5
    public static class StaticMethods
    {
        // Token: 0x0600000C RID: 12 RVA: 0x00002258 File Offset: 0x00000458
        public static bool AccCheckHook(ItemSlot.orig_AccCheck orig, Item item, int slot)
        {
            if (item.type != ModContent.ItemType<GodSoul>())
            {
                return orig.Invoke(item, slot);
            }
            Player player = Main.player[Main.myPlayer];
            if (slot != -1)
            {
                if (player.armor[slot].IsTheSameAs(item))
                {
                    return false;
                }
                if (player.armor[slot].wingSlot > 0 && item.wingSlot > 0)
                {
                    return !ItemLoader.CanEquipAccessory(item, slot);
                }
            }
            for (int i = 0; i < player.armor.Length; i++)
            {
                if (slot < 10 && i < 10)
                {
                    if (item.wingSlot > 0 && player.armor[i].wingSlot > 0)
                    {
                        return true;
                    }
                    if (slot >= 10 && i >= 10 && item.wingSlot > 0 && player.armor[i].wingSlot > 0)
                    {
                        return true;
                    }
                }
            }
            return !ItemLoader.CanEquipAccessory(item, slot);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002328 File Offset: 0x00000528
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float value)
        {
            Vector2 result;
            result.X = MathHelper.Lerp(v1.X, v2.X, value);
            result.Y = MathHelper.Lerp(v1.Y, v2.Y, value);
            return result;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002368 File Offset: 0x00000568
        public static void DrawInItemSlot(SpriteBatch spriteBatch, Item item, Vector2 position, float scale = 1f)
        {
            float inventoryScale = Main.inventoryScale * scale;
            Vector2 vector = Main.inventoryBackTexture.Size() * inventoryScale;
            if (item.type > 0 && item.stack > 0)
            {
                Texture2D ItemTexture = Main.itemTexture[item.type];
                Rectangle frame;
                if (Main.itemAnimations[item.type] != null)
                {
                    frame = Main.itemAnimations[item.type].GetFrame(ItemTexture);
                }
                else
                {
                    frame = ItemTexture.Frame(1, 1, 0, 0);
                }
                float num8 = 1f;
                float num9 = 1f;
                if (frame.Width > 32 || frame.Height > 32)
                {
                    if (frame.Width > frame.Height)
                    {
                        num9 = 32f / frame.Width;
                    }
                    else
                    {
                        num9 = 32f / frame.Height;
                    }
                }
                num9 *= inventoryScale;
                Vector2 position2 = position + vector / 2f - frame.Size() * num9 / 2f;
                Vector2 origin = frame.Size() * (num8 / 2f - 0.5f);
                if (ItemLoader.PreDrawInInventory(item, spriteBatch, position2, frame, item.GetAlpha(Color.White), item.GetColor(Color.White), origin, num9 * num8))
                {
                    spriteBatch.Draw(ItemTexture, position2, new Rectangle?(frame), item.GetAlpha(Color.White), 0f, origin, num9 * num8, 0, 0f);
                    if (item.color != Color.Transparent)
                    {
                        spriteBatch.Draw(ItemTexture, position2, new Rectangle?(frame), item.GetColor(Color.White), 0f, origin, num9 * num8, 0, 0f);
                    }
                }
                ItemLoader.PostDrawInInventory(item, spriteBatch, position2, frame, item.GetAlpha(Color.White), item.GetColor(Color.White), origin, num9 * num8);
                if (ItemID.Sets.TrapSigned[item.type])
                {
                    spriteBatch.Draw(Main.wireTexture, position + new Vector2(40f, 40f) * inventoryScale, new Rectangle?(new Rectangle(4, 58, 8, 8)), Color.White, 0f, new Vector2(4f), 1f, 0, 0f);
                }
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000025A0 File Offset: 0x000007A0
        public static Vector2 GetSlotPosInInventory(int Slot)
        {
            int x = Slot % 10;
            int y = Slot / 10;
            float num = (int)(20f + x * 56 * Main.inventoryScale);
            int PosY = (int)(20f + y * 56 * Main.inventoryScale);
            return new Vector2(num, PosY);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000025E4 File Offset: 0x000007E4
        public static Vector2 GetSelectedPosInInventory()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i + j * 10 == Main.LocalPlayer.selectedItem)
                    {
                        float num = (int)(20f + i * 56 * Main.inventoryScale);
                        int PosY = (int)(20f + j * 56 * Main.inventoryScale);
                        return new Vector2(num, PosY);
                    }
                }
            }
            return Vector2.Zero;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002650 File Offset: 0x00000850
        public static int FindSpace()
        {
            for (int i = 0; i < 50; i++)
            {
                if (Main.LocalPlayer.inventory[i].IsAir)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
