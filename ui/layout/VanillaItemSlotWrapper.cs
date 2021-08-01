using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace SummonHeart.ui.layout
{
    // This class wraps the vanilla ItemSlot class into a UIElement. The ItemSlot class was made before the UI system was made, so it can't be used normally with UIState. 
    // By wrapping the vanilla ItemSlot class, we can easily use ItemSlot.
    // ItemSlot isn't very modder friendly and operates based on a "Context" number that dictates how the slot behaves when left, right, or shift clicked and the background used when drawn. 
    // If you want more control, you might need to write your own UIElement.
    // I've added basic functionality for validating the item attempting to be placed in the slot via the validItem Func. 
    // See ExamplePersonUI for usage and use the Awesomify chat option of Example Person to see in action.
    internal class VanillaItemSlotWrapper : UIElement
    {
        internal Item _accItem;
        internal Item[] singleSlotArray = new Item[1];
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;
        internal Vector2 _pos;
        public VanillaItemSlotWrapper(Vector2 pos, Item accItem, float scale = 1f)
        {
            _context = 10;
            _scale = scale;
            _accItem = accItem;
            _pos = pos;
            Width.Set(Main.inventoryBackTexture.Width * scale, 0f);
            Height.Set(Main.inventoryBackTexture.Height * scale, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();
            //Rectangle r = new Rectangle(0, 0, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.armorHide = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref _accItem, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            ItemSlot.Draw(spriteBatch, ref _accItem, _context, rectangle.TopLeft(), Color.LightBlue);
            Main.inventoryScale = oldScale;
        }

        /*protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();
            //Rectangle r = new Rectangle(0, 0, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.armorHide = true;
                this.singleSlotArray[0] = _accItem;
                ItemSlot.Handle(this.singleSlotArray, 10, 0);
                _accItem = this.singleSlotArray[0];
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            this.singleSlotArray[0] = _accItem;
            ItemSlot.Draw(Main.spriteBatch, this.singleSlotArray, 10, 0, _pos, default(Color));

        }*/
    }
}