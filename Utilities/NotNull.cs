using System;
using Terraria.World.Generation;

namespace SummonHeart.Utilities
{
    public class NotNull : GenCondition
    {
        protected override bool CheckValidity(int x, int y)
        {
            return _tiles[x, y] != null;
        }
    }
}
