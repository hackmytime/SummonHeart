using System;
using SummonHeart.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Buffs.Debuffs
{
    public class Drugged : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Drugged");
            Description.SetDefault("Damage enemies nearby");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<AmmoGlobalNPC>().apDrugged = true;
        }
    }
}
