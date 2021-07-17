using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace SummonHeart.ui
{
    internal class DeathBar : UIState
    {
        public DeathResourceBar killbar;
        public static bool visible = false;

        public override void OnInitialize()
        {
            killbar = new DeathResourceBar();
           /* killbar.Top.Set(50, 0f);
            killbar.Left.Set(-192, 0f);*/
            killbar.OnMouseDown += new MouseEvent(DragStart);
            killbar.OnMouseUp += new MouseEvent(DragEnd);
            Append(killbar);
        }

        Vector2 _offset;
        public bool dragging = false;
        public bool canDrag = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (canDrag)
            {
                _offset = new Vector2(evt.MousePosition.X - killbar.Left.Pixels, evt.MousePosition.Y - killbar.Top.Pixels);
                dragging = true;
            }
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            if (canDrag)
            {
                Vector2 end = evt.MousePosition;
                dragging = false;

                killbar.Left.Set(end.X - _offset.X, 0f);
                killbar.Top.Set(end.Y - _offset.Y, 0f);

                Recalculate();
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY);
            if (killbar.area.ContainsPoint(mousePosition))
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
                killbar.Left.Set(mousePosition.X - _offset.X, 0f);
                killbar.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
    }
}