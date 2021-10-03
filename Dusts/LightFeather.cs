using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Dusts
{
    // Token: 0x020000FC RID: 252
    public class LightFeather : ModDust
    {
        // Token: 0x060005BA RID: 1466 RVA: 0x0002D55F File Offset: 0x0002B75F
        /*public override bool Autoload(ref string name, ref string texture)
        {
            texture = "EpicBattleFantasyUltimate/Dusts/LightFeather";
            return mod.Properties.Autoload;
        }*/

        // Token: 0x060005BB RID: 1467 RVA: 0x0002D4DF File Offset: 0x0002B6DF
        public override void SetDefaults()
        {
            updateType = 226;
        }

        // Token: 0x060005BC RID: 1468 RVA: 0x0002D578 File Offset: 0x0002B778
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 32, 32);
            dust.alpha = 0;
        }

        // Token: 0x060005BD RID: 1469 RVA: 0x0002D594 File Offset: 0x0002B794
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            if (dust.noGravity)
            {
                dust.velocity *= 0.93f;
                if (dust.fadeIn == 0f)
                {
                    dust.scale += 0.0025f;
                }
            }
            dust.scale -= 0.01f;
            dust.alpha++;
            return true;
        }
    }
}
