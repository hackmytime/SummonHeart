using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using SummonHeart.XiuXianModule.Entities;

namespace SummonHeart.Buffs.XiuXian.DanYao
{
    public class HuaYingBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("化婴丹生效中");
            Description.SetDefault("灵力吸收速度翻16倍");
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<RPGPlayer>().danYaoMult = 16;
        }
    }
}