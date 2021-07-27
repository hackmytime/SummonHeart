using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace SummonHeart.ui.Bar
{
    internal class AngerBar : UIState
    {
        public AngerResourceBar angerbar;
        public static bool visible = false;

        public override void OnInitialize()
        {
            angerbar = new AngerResourceBar();
            angerbar.OnMouseDown += new MouseEvent(DragStart);
            angerbar.OnMouseUp += new MouseEvent(DragEnd);
            Append(angerbar);
        }

        Vector2 _offset;
        public bool dragging = false;
        public bool canDrag = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (canDrag)
            {
                _offset = new Vector2(evt.MousePosition.X - angerbar.Left.Pixels, evt.MousePosition.Y - angerbar.Top.Pixels);
                dragging = true;
            }
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            if (canDrag)
            {
                Vector2 end = evt.MousePosition;
                dragging = false;

                angerbar.Left.Set(end.X - _offset.X, 0f);
                angerbar.Top.Set(end.Y - _offset.Y, 0f);

                Recalculate();
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY);
            if (angerbar.area.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
                canDrag = true;
            }
            else
            {
                canDrag = false;
            }
            if (dragging && canDrag)
            {
                angerbar.Left.Set(mousePosition.X - _offset.X, 0f);
                angerbar.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
    }
}