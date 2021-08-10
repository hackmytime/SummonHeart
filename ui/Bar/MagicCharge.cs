using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart;
using SummonHeart.Items.Weapons.Sabres;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.ui.Bar
{
    // Token: 0x0200000F RID: 15
    internal class MagicCharge : UIState
    {
        // Token: 0x06000087 RID: 135 RVA: 0x000129F8 File Offset: 0x00010BF8
        public override void OnInitialize()
        {
            area = new UIElement();
            /*area.Left.Set(-area.Width.Pixels - 1050f, 1f);
            area.Top.Set(580f, 0f);*/

            area.Left.Set(Main.screenWidth / 2 - 182 / 2, 0);
            area.Top.Set(Main.screenHeight / 2 - 60 / 2 + 80, 0);

            area.Width.Set(182f, 0f);
            area.Height.Set(60f, 0f);
            barFrame = new UIImage(ModContent.GetTexture("SummonHeart/ui/Bar/MagicChargeFrame"));
            barFrame.Left.Set(22f, 0f);
            barFrame.Top.Set(0f, 0f);
            barFrame.Width.Set(138f, 0f);
            barFrame.Height.Set(34f, 0f);
            text = new UIText("0/0", 0.8f, false);
            text.Width.Set(138f, 0f);
            text.Height.Set(34f, 0f);
            text.Top.Set(40f, 0f);
            text.Left.Set(0f, 0f);
            gradientA = new Color(205, 205, 180);
            gradientB = new Color(245, 205, 77);
            finalColor = Color.Purple;
            area.Append(text);
            area.Append(barFrame);
            Append(area);
        }

        // Token: 0x06000088 RID: 136 RVA: 0x00012BFA File Offset: 0x00010DFA
        public override void Draw(SpriteBatch spriteBatch)
        {
            SummonHeartPlayer mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (Main.LocalPlayer.HeldItem.modItem is Raiden && mp.PlayerClass == 2)
            {
            }
            else if
            (mp.magicChargeActive)
            {
            }
            else
            {
                return;
            }
            base.Draw(spriteBatch);
        }

        // Token: 0x06000089 RID: 137 RVA: 0x00012C18 File Offset: 0x00010E18
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            SummonHeartPlayer mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            float quotient = mp.magicCharge / mp.magicChargeMax;
            quotient = Utils.Clamp(quotient, 0f, 1f);
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 12;
            hitbox.Width -= 24;
            hitbox.Y += 8;
            hitbox.Height -= 16;
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i++)
            {
                float percent = i / (float)(right - left);
                spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
                if (i >= 113)
                {
                    spriteBatch.Draw(Main.magicPixel, new Rectangle(left, hitbox.Y, 113, hitbox.Height), finalColor);
                }
            }
        }

        // Token: 0x0600008A RID: 138 RVA: 0x00012D2F File Offset: 0x00010F2F
        public override void Update(GameTime gameTime)
        {
            SummonHeartPlayer mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if(Main.LocalPlayer.HeldItem.modItem is Raiden && mp.PlayerClass == 2)
            {
                text.SetText($"刺杀技能：{mp.killResourceSkillCount}个 凝练杀意：{mp.magicCharge} / {mp.magicChargeMax}");
            }
            else if
            (mp.magicChargeActive)
            {
                text.SetText($"充能魔法：{mp.magicChargeCount}个 充能：{mp.magicCharge} / {mp.magicChargeMax}");
            }
            else
            {
                return;
            }
            base.Update(gameTime);
        }

        // Token: 0x04000147 RID: 327
        private UIText text;

        // Token: 0x04000148 RID: 328
        private UIElement area;

        // Token: 0x04000149 RID: 329
        private UIImage barFrame;

        // Token: 0x0400014A RID: 330
        private Color gradientA;

        // Token: 0x0400014B RID: 331
        private Color gradientB;

        // Token: 0x0400014C RID: 332
        private Color finalColor;
    }
}
