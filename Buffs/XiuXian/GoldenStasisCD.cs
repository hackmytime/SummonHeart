using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Buffs.XiuXian
{
    public class GoldenStasisCD : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("修炼冷却");
            Description.SetDefault("你现在还不能修炼");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            canBeCleared = false;
        }
    }
}