using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;

namespace SummonHeart.Extensions
{
    // Token: 0x02000033 RID: 51
    public static class RandomExtensions
    {
        // Token: 0x0600014E RID: 334 RVA: 0x00010550 File Offset: 0x0000E750
        public static Vector2 NextVector2(this UnifiedRandom rng, float minLength, float maxLength)
        {
            return rng.NextVector2Unit(0f, 6.2831855f) * rng.NextFloat(minLength, maxLength);
        }

        // Token: 0x0600014F RID: 335 RVA: 0x0001056F File Offset: 0x0000E76F
        public static Vector2 NextVector2Range(this UnifiedRandom rng, Vector2 min, Vector2 max)
        {
            return min + new Vector2(rng.NextFloat(max.X - min.X), rng.NextFloat(max.Y - min.Y));
        }

        // Token: 0x06000150 RID: 336 RVA: 0x000105A2 File Offset: 0x0000E7A2
        public static float NextFloatRange(this UnifiedRandom rng, float range)
        {
            return rng.NextFloat(-range, range);
        }

        // Token: 0x06000151 RID: 337 RVA: 0x000105AD File Offset: 0x0000E7AD
        public static bool NextChance(this UnifiedRandom rng, double chance)
        {
            return rng.NextDouble() <= chance;
        }

        // Token: 0x06000152 RID: 338 RVA: 0x000105BB File Offset: 0x0000E7BB
        public static T Choose<T>(this UnifiedRandom rng, T a, T b)
        {
            if (!rng.NextChance(0.5))
            {
                return b;
            }
            return a;
        }

        // Token: 0x06000153 RID: 339 RVA: 0x000105D4 File Offset: 0x0000E7D4
        public static T Choose<T>(this UnifiedRandom rng, T a, T b, T c)
        {
            int num = rng.Next(3);
            if (num == 0)
            {
                return a;
            }
            if (num != 1)
            {
                return c;
            }
            return b;
        }

        // Token: 0x06000154 RID: 340 RVA: 0x000105F7 File Offset: 0x0000E7F7
        public static T Choose<T>(this UnifiedRandom rng, params T[] list)
        {
            return list[rng.Next(list.Length)];
        }
    }
}
