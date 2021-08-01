using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.body;
using SummonHeart.costvalues;
using SummonHeart.Extensions;
using SummonHeart.Items;
using SummonHeart.ui.layout;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.ui
{
    class PanelGodSoul : UIState
    {
        public static bool visible = false;
      
        public override void OnInitialize()
        {
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            ((AllItemsMenu)SummonHeartMod.Instance.GetGlobalItem("AllItemsMenu")).DrawUpdateExtraAccessories(spriteBatch);
        }

        public bool needValidate = false;


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
