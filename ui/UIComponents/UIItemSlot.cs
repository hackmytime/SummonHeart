using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace SummonHeart.ui.UIComponents
{
    public class UIItemSlot : UIElement
    {
        public event Func<Item, bool> PreItemChange;

        public event Action<Item> PostItemChange;

        public event Func<Item, bool> PreDrawItemSlot;

        public event Action<Item> PostDrawItemSlot;

        public event Action OnMouseHover;

        public UIItemSlot(Texture2D bgTexture = null, float scale = 1f, int context = 0)
        {
            validItem = null;
            backgroundTexture = bgTexture ?? Main.inventoryBackTexture;
            this.context = context;
            this.scale = scale;
            item = new Item();
            item.SetDefaults(0, false);
            Width.Set(backgroundTexture.Width * scale, 0f);
            Height.Set(backgroundTexture.Height * scale, 0f);
        }

        // Token: 0x06000492 RID: 1170 RVA: 0x00017700 File Offset: 0x00015900
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = GetDimensions().ToRectangle();
            if (IsMouseHovering)
            {
                Action onMouseHover = OnMouseHover;
                if (onMouseHover != null)
                {
                    onMouseHover();
                }
            }
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (validItem == null || validItem(Main.mouseItem))
                {
                    Func<Item, bool> preItemChange = PreItemChange;
                    bool? pre = preItemChange != null ? new bool?(preItemChange(item)) : null;
                    if (pre != null)
                    {
                        bool? flag = pre;
                        bool flag2 = true;
                        if (!(flag.GetValueOrDefault() == flag2 & flag != null))
                        {
                            goto IL_C3;
                        }
                    }
                    ItemSlot.Handle(ref item, context);
                IL_C3:
                    Action<Item> postItemChange = PostItemChange;
                    if (postItemChange != null)
                    {
                        postItemChange(item);
                    }
                }
            }
            Func<Item, bool> preDrawItemSlot = PreDrawItemSlot;
            bool? preDraw = preDrawItemSlot != null ? new bool?(preDrawItemSlot(item)) : null;
            if (preDraw != null)
            {
                bool? flag = preDraw;
                bool flag2 = true;
                if (!(flag.GetValueOrDefault() == flag2 & flag != null))
                {
                    goto IL_143;
                }
            }
            Draw(item, rectangle.TopLeft(), backgroundTexture, scale);
        IL_143:
            Action<Item> postDrawItemSlot = PostDrawItemSlot;
            if (postDrawItemSlot == null)
            {
                return;
            }
            postDrawItemSlot(item);
        }

        // Token: 0x06000493 RID: 1171 RVA: 0x00017866 File Offset: 0x00015A66
        public void PutItemInInventory(Player player = null)
        {
            if (player != null)
            {
                player.PutItemInInventory(item.type, -1);
            }
            else
            {
                Main.LocalPlayer.PutItemInInventory(item.type, -1);
            }
            item.TurnToAir();
        }

        // Token: 0x06000494 RID: 1172 RVA: 0x000178A0 File Offset: 0x00015AA0
        public void ChangeItem(Item newItem)
        {
            if (newItem == null)
            {
                throw new ArgumentNullException();
            }
            Func<Item, bool> preItemChange = PreItemChange;
            bool? pre = preItemChange != null ? new bool?(preItemChange(newItem)) : null;
            if (pre != null)
            {
                bool? flag = pre;
                bool flag2 = true;
                if (!(flag.GetValueOrDefault() == flag2 & flag != null))
                {
                    goto IL_52;
                }
            }
            item = newItem;
        IL_52:
            Action<Item> postItemChange = PostItemChange;
            if (postItemChange == null)
            {
                return;
            }
            postItemChange(item);
        }

        // Token: 0x06000495 RID: 1173 RVA: 0x00017918 File Offset: 0x00015B18
        public void ChangeItem(int type)
        {
            if (type <= 0)
            {
                throw new ArgumentException();
            }
            Func<Item, bool> preItemChange = PreItemChange;
            bool? pre = preItemChange != null ? new bool?(preItemChange(item)) : null;
            if (pre != null)
            {
                bool? flag = pre;
                bool flag2 = true;
                if (!(flag.GetValueOrDefault() == flag2 & flag != null))
                {
                    goto IL_5D;
                }
            }
            item.type = type;
        IL_5D:
            Action<Item> postItemChange = PostItemChange;
            if (postItemChange == null)
            {
                return;
            }
            postItemChange(item);
        }

        // Token: 0x06000496 RID: 1174 RVA: 0x00017998 File Offset: 0x00015B98
        public void Draw(Item item, Vector2 position, Texture2D bg = null, float scale = 1f)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Player player = Main.player[Main.myPlayer];
            Texture2D background = bg ?? Main.inventoryBackTexture;
            Vector2 bgSize = background.Size() * scale;
            if (background == GFX.GFX.NOTHING)
            {
                bgSize = new Vector2(GetDimensions().Width, GetDimensions().Height);
            }
            spriteBatch.Draw(background, position, null, Main.inventoryBack, 0f, default, scale, 0, 0f);
            if (item.type > 0 && item.stack > 0)
            {
                Texture2D itemTexture = Main.itemTexture[item.type];
                Rectangle frame = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(itemTexture) : itemTexture.Frame(1, 1, 0, 0);
                Color itemColor = Color.White;
                float lightScale = 1f;
                float itemScale = 1f;
                ItemSlot.GetItemLight(ref itemColor, ref lightScale, item, false);
                if (frame.Width > 32 || frame.Height > 32)
                {
                    if (frame.Width > frame.Height)
                    {
                        itemScale = 32f / frame.Width;
                    }
                    else
                    {
                        itemScale = 32f / frame.Height;
                    }
                }
                itemScale *= scale;
                Vector2 itemPos = position + bgSize / 2f - frame.Size() * itemScale / 2f;
                Vector2 itemOrigin = frame.Size() * (lightScale / 2f - 0.5f);
                if (ItemLoader.PreDrawInInventory(item, spriteBatch, itemPos, frame, item.GetAlpha(itemColor), item.GetColor(Color.White), itemOrigin, itemScale * lightScale))
                {
                    spriteBatch.Draw(itemTexture, itemPos, new Rectangle?(frame), item.GetAlpha(itemColor), 0f, itemOrigin, itemScale * lightScale, 0, 0f);
                    if (item.color != Color.Transparent)
                    {
                        spriteBatch.Draw(itemTexture, itemPos, new Rectangle?(frame), item.GetColor(Color.White), 0f, itemOrigin, itemScale * lightScale, 0, 0f);
                    }
                }
                ItemLoader.PostDrawInInventory(item, spriteBatch, itemPos, frame, item.GetAlpha(itemColor), item.GetColor(Color.White), itemOrigin, itemScale * lightScale);
                if (ItemID.Sets.TrapSigned[item.type])
                {
                    spriteBatch.Draw(Main.wireTexture, position + new Vector2(40f, 40f) * scale, new Rectangle?(new Rectangle(4, 58, 8, 8)), Color.White, 0f, new Vector2(4f), 1f, 0, 0f);
                }
                if (item.stack > 1)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, item.stack.ToString(), position + new Vector2(10f, 26f) * scale, Color.White, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);
                }
                if (item.potion)
                {
                    Vector2 pos = position + background.Size() * scale / 2f - Main.cdTexture.Size() * scale / 2f;
                    Color color = item.GetAlpha(Color.White) * (player.potionDelay / (float)player.potionDelayTime);
                    spriteBatch.Draw(Main.cdTexture, pos, null, color, 0f, default, itemScale, 0, 0f);
                }
                if (item.expertOnly && !Main.expertMode)
                {
                    Vector2 pos2 = position + background.Size() * scale / 2f - Main.cdTexture.Size() * scale / 2f;
                    spriteBatch.Draw(Main.cdTexture, pos2, null, Color.White, 0f, default, itemScale, 0, 0f);
                }
            }
        }

        public Item item;

        public Func<Item, bool> validItem;

        public Texture2D backgroundTexture;

        public float scale;

        public int context;
    }
}
