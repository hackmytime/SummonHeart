using SummonHeart.Extensions;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Buffs
{
    class SHGlobalBuff : GlobalBuff
    {
        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            SummonHeartPlayer mp = Main.LocalPlayer.SH();
            if (mp.infiniBuffDic.Keys.Contains(type))
                tip = "此Buff已被无限法则转化为自身被动：" + tip;
            base.ModifyBuffTip(type, ref tip, ref rare);
        }

        public override void Update(int type, Player player, ref int buffIndex)
        {
            base.Update(type, player, ref buffIndex);
        }
    }
}
