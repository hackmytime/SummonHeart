using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Dusts
{
    // Token: 0x020000FB RID: 251
    public class LightBubble : ModDust
    {
        // Token: 0x060005B5 RID: 1461 RVA: 0x0002D4C6 File Offset: 0x0002B6C6
       /* public override bool Autoload(ref string name, ref string texture)
        {
            texture = "EpicBattleFantasyUltimate/Dusts/LightBubble";
            return mod.Properties.Autoload;
        }*/

        // Token: 0x060005B6 RID: 1462 RVA: 0x0002D4DF File Offset: 0x0002B6DF
        public override void SetDefaults()
        {
            updateType = 226;
        }

        // Token: 0x060005B7 RID: 1463 RVA: 0x0002D4EC File Offset: 0x0002B6EC
        public override void OnSpawn(Dust dust)
        {
            startX = 10;
            dust.frame = new Rectangle(0, 0, 24, 24);
            dust.alpha = 0;
        }

        // Token: 0x060005B8 RID: 1464 RVA: 0x0002D510 File Offset: 0x0002B710
        public override bool Update(Dust dust)
        {
            float xOffset = (float)(Math.Cos(MathHelper.ToRadians(counter)) * distance);
            int num = startX;
            counter++;
            return true;
        }

        // Token: 0x040001E5 RID: 485
        public int distance = 100;

        // Token: 0x040001E6 RID: 486
        public int counter;

        // Token: 0x040001E7 RID: 487
        public int startX;
    }
}
