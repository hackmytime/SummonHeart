using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace SummonHeart.ui.UIComponents
{
    // Token: 0x020000C6 RID: 198
    public class UITextHoverImageButton : UIImageButton
    {
        // Token: 0x170000A2 RID: 162
        // (get) Token: 0x06000497 RID: 1175 RVA: 0x00017DC3 File Offset: 0x00015FC3
        // (set) Token: 0x06000498 RID: 1176 RVA: 0x00017DCB File Offset: 0x00015FCB
        public string HoverText { get; private set; }

        // Token: 0x14000008 RID: 8
        // (add) Token: 0x06000499 RID: 1177 RVA: 0x00017DD4 File Offset: 0x00015FD4
        // (remove) Token: 0x0600049A RID: 1178 RVA: 0x00017E0C File Offset: 0x0001600C
        public event Func<string, bool> PreTextChange;

        // Token: 0x14000009 RID: 9
        // (add) Token: 0x0600049B RID: 1179 RVA: 0x00017E44 File Offset: 0x00016044
        // (remove) Token: 0x0600049C RID: 1180 RVA: 0x00017E7C File Offset: 0x0001607C
        public event Action<string> PostTextChange;

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x0600049D RID: 1181 RVA: 0x00017EB4 File Offset: 0x000160B4
        // (remove) Token: 0x0600049E RID: 1182 RVA: 0x00017EEC File Offset: 0x000160EC
        public event Func<bool, SpriteBatch, bool> PreDrawHoverText;

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x0600049F RID: 1183 RVA: 0x00017F24 File Offset: 0x00016124
        // (remove) Token: 0x060004A0 RID: 1184 RVA: 0x00017F5C File Offset: 0x0001615C
        public event Action<SpriteBatch> PostDrawHoverText;

        // Token: 0x060004A1 RID: 1185 RVA: 0x00017F91 File Offset: 0x00016191
        public UITextHoverImageButton(Texture2D texture, string hoverText) : base(texture)
        {
            HoverText = hoverText;
        }

        // Token: 0x060004A2 RID: 1186 RVA: 0x00017FA4 File Offset: 0x000161A4
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Func<bool, SpriteBatch, bool> preDrawHoverText = PreDrawHoverText;
            bool? flag = preDrawHoverText != null ? new bool?(preDrawHoverText(IsMouseHovering, spriteBatch)) : null;
            if (IsMouseHovering)
            {
                bool? flag2 = flag;
                bool flag3 = true;
                if (flag2.GetValueOrDefault() == flag3 & flag2 != null || flag == null)
                {
                    Main.hoverItemName = HoverText;
                    Action<SpriteBatch> postDrawHoverText = PostDrawHoverText;
                    if (postDrawHoverText == null)
                    {
                        return;
                    }
                    postDrawHoverText(spriteBatch);
                }
            }
        }

        // Token: 0x060004A3 RID: 1187 RVA: 0x00018024 File Offset: 0x00016224
        public void ChangeHoverText(string newText)
        {
            if (newText != HoverText && PreTextChange(newText))
            {
                HoverText = newText;
                PostTextChange(HoverText);
            }
        }
    }
}
