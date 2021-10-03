using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace SummonHeart.ui.UIComponents
{
    public class UIHoverImageButton : UIImageButton
    {
        public event Action<SpriteBatch> OnHover;

        public UIHoverImageButton(Texture2D texture) : base(texture)
        {
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (IsMouseHovering)
            {
                Action<SpriteBatch> onHover = OnHover;
                if (onHover == null)
                {
                    return;
                }
                onHover(spriteBatch);
            }
        }
    }
}
