using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using SummonHeart.XiuXianModule.Entities;

namespace SummonHeart.Buffs.XiuXian.DanYao
{
    public class ShengJiBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("生机丹生效中");
            Description.SetDefault("每秒回复生命1000点");
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<RPGPlayer>().lifeHeal = 1000;
        }
    }
}