using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.NPCs
{
    public class GoddessGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (firstLoot && npc.type != 439 && SummonHeartWorld.GoddessMode)
            {
                firstLoot = false;
                for (int i = 1; i < 2; i++)
                {
                    npc.NPCLoot();
                }
            }
            firstLoot = false;
        }

        private bool firstLoot = true;
    }
}
