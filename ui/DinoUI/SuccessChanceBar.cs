using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace SummonHeart.ui.DinoUI
{
    public class SuccessChanceBar : UIImage
    {
        public SuccessChanceBar(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            int MaxBars = ChanceBars + ValueBars;
            int barHeight = (area.Height - (MaxBars - 1) * Padding) / MaxBars;
            for (int i = 0; i < MaxBars; i++)
            {
                spriteBatch.Draw(Main.magicPixel, new Rectangle((int)GetDimensions().X + area.Left, (int)GetDimensions().Y + area.Top + i * Padding + i * barHeight, area.Width, barHeight), DarkBarColor);
            }
            if (SuccessChance + FossilValue > 0f)
            {
                float litBarCount = ChanceBars * SuccessChance + ValueBars * FossilValue;
                int count = 1;
                while (count <= (int)litBarCount && count <= MaxBars)
                {
                    spriteBatch.Draw(Main.magicPixel, new Rectangle((int)GetDimensions().X + area.Left, (int)GetDimensions().Y + area.Top + (MaxBars - count) * Padding + (MaxBars - count) * barHeight, area.Width, barHeight), LitBarColor);
                    count++;
                }
                if (count <= MaxBars)
                {
                    Color mix = Color.Lerp(DarkBarColor, LitBarColor, litBarCount - count + 1f);
                    spriteBatch.Draw(Main.magicPixel, new Rectangle((int)GetDimensions().X + area.Left, (int)GetDimensions().Y + area.Top + (MaxBars - count) * Padding + (MaxBars - count) * barHeight, area.Width, barHeight), mix);
                }
            }
        }

        public float SuccessChance;

        // Token: 0x0400017B RID: 379
        public float FossilValue;

        // Token: 0x0400017C RID: 380
        public int ChanceBars = 4;

        // Token: 0x0400017D RID: 381
        public int ValueBars = 2;

        // Token: 0x0400017E RID: 382
        public int Padding = 3;

        // Token: 0x0400017F RID: 383
        public Rectangle area = new Rectangle(10, 29, 40, 110);

        // Token: 0x04000180 RID: 384
        public Color LitBarColor = new Color(35, 151, 248);

        // Token: 0x04000181 RID: 385
        public Color DarkBarColor = new Color(48, 56, 111);
    }
}
