using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Buffs.GoodBuff
{
    public class DemonDefBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("DemonDefBuff");
            Description.SetDefault("You are blessed by the demon God. The damage reduction rate is + 3");
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神的庇佑");
            Description.AddTranslation(GameCulture.Chinese, "你被魔神所庇佑，减伤倍率+3。击杀任意boss后，庇佑消失。");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            mp.myDamageReduceMult += 3f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.lifeRegen -= 20;
        }
    }
}