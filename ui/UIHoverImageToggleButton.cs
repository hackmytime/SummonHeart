﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SummonHeart.ui
{
    // This UIHoverImageButton class inherits from UIImageButton. 
    // Inheriting is a great tool for UI design. 
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered. 
    internal class UIHoverImageToggleButton : UIImageButton
    {
        internal string HoverTextChecked;
        internal string HoverTextUnchecked;

        public bool IsChecked = false;

        public delegate void CheckEvent(bool val);

        public event CheckEvent OnChecked;

        Texture2D texture_checked;
        Texture2D texture_unchecked;
        public UIHoverImageToggleButton(Texture2D texture_checked, Texture2D texture_unchecked, string hoverTextchecked, string hoverTextunchecked, bool canToggle) : base(texture_unchecked)
        {
            HoverTextChecked = hoverTextchecked;
            HoverTextUnchecked = hoverTextunchecked;
            if(canToggle)
                OnClick += new MouseEvent(PlayButtonClicked);
            this.texture_checked = texture_checked;
            this.texture_unchecked = texture_unchecked;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering)
            {

                TooltipPanel.Instance.Show(this);
                TooltipPanel.Instance.X = Main.mouseX;
                TooltipPanel.Instance.Y = Main.mouseY;




            }
            else
            {
                TooltipPanel.Instance.Hide(this);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (IsMouseHovering)
            {

            }
        }

        private void PlayButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
           IsChecked = !IsChecked;

            if (IsChecked)
            {
                SetImage(texture_checked);
            }
            else
            {
                SetImage(texture_unchecked);
            }

            OnChecked?.Invoke(IsChecked);
        }
    }
}