using SummonHeart.Projectiles.Weapon;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Buffs.Weapon
{
    public class BladeStaffBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Enchanted Daggers");
            Description.SetDefault("魔剑·神灭为魔神之子护道");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DemonFlySwordMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
                return;
            }
            player.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
