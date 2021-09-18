using System;
using SummonHeart.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Buffs.Debuffs
{
    public class Cactus : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spiky Shield");
            Description.SetDefault("Enemies nearby are hurt");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<AmmoGlobalNPC>().apCactus = true;
        }
    }
}
