using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace SummonHeart.Extensions
{
    // Token: 0x020005C8 RID: 1480
    public struct Gradient
    {
        // Token: 0x06001EDB RID: 7899 RVA: 0x000E83D0 File Offset: 0x000E65D0
        public static Gradient Linear(bool wrap, params Color[] colors)
        {
            Gradient gradient = default;
            int length = colors.Length;
            gradient.steps = new Step[length + wrap.ToInt()];
            for (int i = 0; i < length; i++)
            {
                gradient.steps[i].position = i / (float)length;
                gradient.steps[i].color = colors[i];
            }
            if (wrap)
            {
                gradient.steps[length] = new Step
                {
                    color = gradient.steps[0].color,
                    position = 1f
                };
            }
            gradient.length = 1f / length;
            return gradient;
        }

        // Token: 0x06001EDC RID: 7900 RVA: 0x000E8484 File Offset: 0x000E6684
        public Color Sample(float progress)
        {
            Step low = steps[0];
            Step high = steps[steps.Length - 1];
            for (int i = 0; i < steps.Length - 1; i++)
            {
                if (steps[i].position <= progress && steps[i + 1].position > progress)
                {
                    high = steps[i + 1];
                    low = steps[i];
                    break;
                }
            }
            return Color.Lerp(low.color, high.color, (progress - low.position) / (high.position - low.position));
        }

        // Token: 0x040005CA RID: 1482
        private Step[] steps;

        // Token: 0x040005CB RID: 1483
        public float length;

        // Token: 0x020005C9 RID: 1481
        private struct Step
        {
            // Token: 0x040005CC RID: 1484
            public Color color;

            // Token: 0x040005CD RID: 1485
            public float position;
        }
    }
}
