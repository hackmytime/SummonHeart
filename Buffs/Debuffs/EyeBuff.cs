using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Buffs.Debuffs
{
    public class EyeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("EyeBuff");
            Description.SetDefault("Your soul is cursed by divine wrath");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            DisplayName.AddTranslation(GameCulture.Chinese, "死亡之眼");
            Description.AddTranslation(GameCulture.Chinese, "你被死亡之眼所注视");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //defense removed, endurance removed, colossal DOT (45 per second)
            //player.moonLeech = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.lifeRegen -= 20;
        }
    }
}