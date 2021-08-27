using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Buffs
{
    class SHGlobalBuff : GlobalBuff
    {
        public override void CustomBuffTipSize(string buffTip, List<Vector2> sizes)
        {
            base.CustomBuffTipSize(buffTip, sizes);
        }

        public override void DrawCustomBuffTip(string buffTip, SpriteBatch spriteBatch, int originX, int originY)
        {
            base.DrawCustomBuffTip(buffTip, spriteBatch, originX, originY);
        }

        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            base.ModifyBuffTip(type, ref tip, ref rare);
        }

        public override void Update(int type, Player player, ref int buffIndex)
        {
            base.Update(type, player, ref buffIndex);
        }
    }
}
