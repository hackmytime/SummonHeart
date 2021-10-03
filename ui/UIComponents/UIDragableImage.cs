using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SummonHeart.ui.UIComponents
{
    // Token: 0x020000C1 RID: 193
    public class UIDragableImage : UIImage
    {
        // Token: 0x17000099 RID: 153
        // (get) Token: 0x0600045F RID: 1119 RVA: 0x00016BCF File Offset: 0x00014DCF
        // (set) Token: 0x06000460 RID: 1120 RVA: 0x00016BD7 File Offset: 0x00014DD7
        private Vector2 Offset { get; set; }

        // Token: 0x1700009A RID: 154
        // (get) Token: 0x06000461 RID: 1121 RVA: 0x00016BE0 File Offset: 0x00014DE0
        // (set) Token: 0x06000462 RID: 1122 RVA: 0x00016BE8 File Offset: 0x00014DE8
        public bool Dragging { get; private set; }

        // Token: 0x1700009B RID: 155
        // (get) Token: 0x06000463 RID: 1123 RVA: 0x00016BF1 File Offset: 0x00014DF1
        // (set) Token: 0x06000464 RID: 1124 RVA: 0x00016BF9 File Offset: 0x00014DF9
        public bool StopItemUse { get; private set; }

        // Token: 0x06000465 RID: 1125 RVA: 0x00016C02 File Offset: 0x00014E02
        public UIDragableImage(Texture2D texture, bool stopItemUse = true) : base(texture)
        {
            StopItemUse = stopItemUse;
        }

        // Token: 0x06000466 RID: 1126 RVA: 0x00016C12 File Offset: 0x00014E12
        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            DragStart(evt);
        }

        // Token: 0x06000467 RID: 1127 RVA: 0x00016C22 File Offset: 0x00014E22
        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            DragEnd(evt);
        }

        // Token: 0x06000468 RID: 1128 RVA: 0x00016C34 File Offset: 0x00014E34
        private void DragStart(UIMouseEvent evt)
        {
            Offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            Dragging = true;
        }

        // Token: 0x06000469 RID: 1129 RVA: 0x00016C84 File Offset: 0x00014E84
        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 end = evt.MousePosition;
            Dragging = false;
            Left.Set(end.X - Offset.X, 0f);
            Top.Set(end.Y - Offset.Y, 0f);
            Recalculate();
        }

        // Token: 0x0600046A RID: 1130 RVA: 0x00016CEC File Offset: 0x00014EEC
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (ContainsPoint(Main.MouseScreen) && StopItemUse)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (Dragging)
            {
                Left.Set(Main.mouseX - Offset.X, 0f);
                Top.Set(Main.mouseY - Offset.Y, 0f);
                Recalculate();
            }
            Rectangle parentSpace = Parent.GetDimensions().ToRectangle();
            Rectangle panelRectTransform = GetDimensions().ToRectangle();
            bool flag = parentSpace.Left > panelRectTransform.Left;
            bool right = parentSpace.Right < panelRectTransform.Right;
            bool top = parentSpace.Top > panelRectTransform.Top;
            bool bottom = parentSpace.Bottom < panelRectTransform.Bottom;
            if (flag || right || top || bottom || !GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0f, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0f, parentSpace.Bottom - Height.Pixels);
                Recalculate();
            }
        }
    }
}
