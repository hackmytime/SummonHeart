using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace SummonHeart.ui.Bar
{
    internal class DamageBar : UIState
    {
        public DamageResourceBar Damagebar;
        public static bool visible = false;

        public override void OnInitialize()
        {
            Damagebar = new DamageResourceBar();
            Damagebar.OnMouseDown += new MouseEvent(DragStart);
            Damagebar.OnMouseUp += new MouseEvent(DragEnd);
            Append(Damagebar);
        }

        Vector2 _offset;
        public bool dragging = false;
        public bool canDrag = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (canDrag)
            {
                _offset = new Vector2(evt.MousePosition.X - Damagebar.Left.Pixels, evt.MousePosition.Y - Damagebar.Top.Pixels);
                dragging = true;
            }
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            if (canDrag)
            {
                Vector2 end = evt.MousePosition;
                dragging = false;

                Damagebar.Left.Set(end.X - _offset.X, 0f);
                Damagebar.Top.Set(end.Y - _offset.Y, 0f);

                Recalculate();
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY);
            if (Damagebar.area.ContainsPoint(mousePosition))
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
                Damagebar.Left.Set(mousePosition.X - _offset.X, 0f);
                Damagebar.Top.Set(mousePosition.Y - _offset.Y, 0f);
                Recalculate();
            }
        }
    }
}