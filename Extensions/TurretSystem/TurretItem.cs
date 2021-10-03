using System;
using Terraria.ModLoader;

namespace SummonHeart.Extensions.TurretSystem
{
    public abstract class TurretItem : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 36;
            item.maxStack = 15;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 2;
            item.createTile = PickTile();
        }

        protected abstract int PickTile();
    }
}
