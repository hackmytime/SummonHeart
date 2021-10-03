using System;
using Microsoft.Xna.Framework;

namespace SummonHeart.Extensions
{
    // Token: 0x0200002D RID: 45
    public static class ColorHelper
    {

        // Token: 0x0600012E RID: 302 RVA: 0x0000F8CB File Offset: 0x0000DACB
        public static Color Alpha(this Color c, float alpha)
        {
            return new Color(c.R, c.G, c.B, (int)(255f * MathHelper.Clamp(alpha, 0f, 1f)));
        }
        public static Color getRandomColor()
        {
            int iSeed = 10;
            Random ro = new Random(10);
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int R = ran.Next(255);
            int G = ran.Next(255);
            int B = ran.Next(255);
            B = (R + G > 400) ? R + G - 400 : B;//0 : 380 - R - G;
            B = (B > 255) ? 255 : B;
            return new Color(R, G, B);
        }

        // Token: 0x0600012F RID: 303 RVA: 0x0000F900 File Offset: 0x0000DB00
        public static Color MultiplyAlpha(this Color c, float alpha)
        {
            return new Color(c.R, c.G, c.B, (int)(c.A / 255f * MathHelper.Clamp(alpha, 0f, 1f) * 255f));
        }

        // Token: 0x04000184 RID: 388
        public static readonly Color Workbench = new Color(191, 142, 111);

        // Token: 0x04000185 RID: 389
        public static readonly Color Picture = new Color(120, 85, 60);

        // Token: 0x04000186 RID: 390
        public static readonly Color FrostbiteFurniture = new Color(87, 144, 165);

        // Token: 0x04000187 RID: 391
        public static readonly Color Tile = new Color(75, 139, 166);

        // Token: 0x04000188 RID: 392
        public static readonly Color Spirit = new Color(106, 50, 215);

        // Token: 0x04000189 RID: 393
        public static readonly Color Boss = new Color(175, 75, 255);

        // Token: 0x0400018A RID: 394
        public static readonly Color GreenChat = new Color(50, 255, 130);
    }
}
